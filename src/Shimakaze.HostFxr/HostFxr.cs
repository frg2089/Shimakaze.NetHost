using System;
using System.Runtime.InteropServices;

namespace Shimakaze;

internal sealed class HostFxr(string path) : IDisposable
{
    public const int Success = 0;
    public const int Success_HostAlreadyInitialized = 1;
    public const int Success_DifferentRuntimeProperties = 2;

    private readonly nint _handle = NativeLibrary.Load(path);
    private bool _disposedValue;

    public unsafe delegate* unmanaged[Cdecl]<int, byte**, InitializeParametersStruct*, out nint, int> InitializeForDotnetCommandLine => field == null
        ? field = (delegate* unmanaged[Cdecl]<int, byte**, InitializeParametersStruct*, out nint, int>)NativeLibrary.GetExport(_handle, "hostfxr_initialize_for_dotnet_command_line")
        : field;
    public unsafe delegate* unmanaged[Cdecl]<byte*, InitializeParametersStruct*, out nint, int> InitializeForRuntimeConfig => field == null
        ? field = (delegate* unmanaged[Cdecl]<byte*, InitializeParametersStruct*, out nint, int>)NativeLibrary.GetExport(_handle, "hostfxr_initialize_for_runtime_config")
        : field;
    public unsafe delegate* unmanaged[Cdecl]<nint, int> RunApp => field == null
        ? field = (delegate* unmanaged[Cdecl]<nint, int>)NativeLibrary.GetExport(_handle, "hostfxr_run_app")
        : field;
    public unsafe delegate* unmanaged[Cdecl]<nint, DelegateType, out nint, int> GetRuntimeDelegate => field == null
        ? field = (delegate* unmanaged[Cdecl]<nint, DelegateType, out nint, int>)NativeLibrary.GetExport(_handle, "hostfxr_get_runtime_delegate")
        : field;
    public unsafe delegate* unmanaged[Cdecl]<nint, int> Close => field == null
        ? field = (delegate* unmanaged[Cdecl]<nint, int>)NativeLibrary.GetExport(_handle, "hostfxr_close")
        : field;

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
            }

            NativeLibrary.Free(_handle);
            _disposedValue = true;
        }
    }

    ~HostFxr()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}


#if NETFRAMEWORK || NETSTANDARD
file static class NativeLibrary
{
    public static nint Load(string libraryPath)
    {
#if !NETFRAMEWORK
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
            return LoadLibraryW(libraryPath);
#if !NETFRAMEWORK
        else
            return dlopen(libraryPath, 2);

        [DllImport("libdl", SetLastError = true)]
        static extern nint dlopen(string fileName, int flags);
#endif
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern nint LoadLibraryW(string lpFileName);
    }

    public static void Free(nint handle)
    {
#if !NETFRAMEWORK
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
            FreeLibrary(handle);
#if !NETFRAMEWORK
        else
            dlclose(handle);

        [DllImport("libdl", SetLastError = true)]
        static extern int dlclose(nint handle);
#endif
        [DllImport("kernel32", SetLastError = true)]
        static extern bool FreeLibrary(nint hModule);

    }

    public static nint GetExport(nint handle, string name)
    {

#if !NETFRAMEWORK
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
            return GetProcAddress(handle, name);
#if !NETFRAMEWORK
        else
            return dlsym(handle, name);

        [DllImport("libdl", SetLastError = true)]
        static extern nint dlsym(nint handle, string symbol);
#endif
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern nint GetProcAddress(nint hModule, string lpProcName);
    }
}
#endif