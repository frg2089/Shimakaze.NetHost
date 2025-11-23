using System;

namespace Shimakaze;

public sealed unsafe class HostContext(HostFxr hostfxr, nint handle) : SafeHandleZeroIsInvalid(handle)
{
    public new int Close() => hostfxr.Close(handle);
    public int GetRuntimeDelegate(DelegateType type, out void** @delegate) => hostfxr.GetRuntimeDelegate(handle, type, out @delegate);
    public int GetRuntimeProperties(ref nuint count, out string?[] keys, out string?[] values)
    {
        var result = hostfxr.GetRuntimeProperties(handle, ref count, out var lpszKeys, out var lpszValues);
        int szKeys = 0;
        while (lpszKeys[szKeys] != null)
            szKeys++;

        int szValues = 0;
        while (lpszValues[szValues] != null)
            szValues++;

        keys = new string[szKeys];
        for (int i = 0; i < keys.Length; i++)
            keys[i] = HostFxr.PtrToString((nint)lpszKeys[i]);

        values = new string[szValues];
        for (int i = 0; i < values.Length; i++)
            values[i] = HostFxr.PtrToString((nint)lpszValues[i]);

        return result;
    }
    public int GetRuntimePropertyValue(string name, out string?[] value)
    {
        var lName = HostFxr.Encoding.GetByteCount(name);
        Span<byte> szName = new byte[lName + HostFxr.CharSize];
        HostFxr.Encoding.GetBytes(name, szName);

        fixed (byte* lpszName = szName)
        {
            var result = hostfxr.GetRuntimePropertyValue(handle, lpszName, out var lpszValue);
            int szValue = 0;
            while (lpszValue[szValue] != null)
                szValue++;

            value = new string[szValue];
            for (int i = 0; i < value.Length; i++)
                value[i] = HostFxr.PtrToString((nint)lpszValue[i]);

            return result;
        }
    }
    public int RunApp() => hostfxr.RunApp(handle);
    public int SetRuntimePropertyValue(string name, string value)
    {
        var cName = HostFxr.Encoding.GetByteCount(name);
        Span<byte> szName = new byte[cName + HostFxr.CharSize];
        HostFxr.Encoding.GetBytes(name, szName);

        var cValue = HostFxr.Encoding.GetByteCount(value);
        Span<byte> szValue = new byte[cValue + HostFxr.CharSize];
        HostFxr.Encoding.GetBytes(value, szValue);

        fixed (byte* lpszName = szName)
        fixed (byte* lpszValue = szValue)
            return hostfxr.SetRuntimePropertyValue(handle, lpszName, lpszValue);
    }
    protected override bool ReleaseHandle() => Close() == 0;
}