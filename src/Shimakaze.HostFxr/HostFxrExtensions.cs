using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Shimakaze;

public static class HostFxrExtensions
{
    public static unsafe int Initialize(this HostFxr hostfxr, string[] args, SafeInitializeParameters? parameters, out HostContext context)
    {
        List<MemoryHandle> handles = new(args.Length + 2);
        try
        {
            byte*[] argv = new byte*[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                var str = args[i];
                var lstr = HostFxr.Encoding.GetByteCount(str);
                byte[] sz = new byte[lstr + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(str, sz);
                var handle = sz.AsMemory().Pin();
                argv[i] = (byte*)handle.Pointer;
                handles.Add(handle);
            }

            InitializeParameters* param = null;
            if (parameters is not null)
            {
                var chp = HostFxr.Encoding.GetByteCount(parameters.HostPath);
                byte[] szhp = new byte[chp + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(parameters.HostPath, szhp);
                var hhp = szhp.AsMemory().Pin();
                handles.Add(hhp);

                var cdr = HostFxr.Encoding.GetByteCount(parameters.DotnetRoot);
                byte[] szdr = new byte[cdr + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(parameters.DotnetRoot, szdr);
                var hdr = szdr.AsMemory().Pin();
                handles.Add(hdr);

                InitializeParameters tmp = new()
                {
                    Size = (nuint)sizeof(InitializeParameters),
                    HostPath = (byte*)hhp.Pointer,
                    DotnetRoot = (byte*)hdr.Pointer,
                };
                param = &tmp;
            }

            fixed (byte** ptr = argv)
            {
                var result = hostfxr.InitializeForDotnetCommandLine(args.Length, ptr, param, out nint hContext);
                context = new(hostfxr, hContext);
                return result;
            }
        }
        finally
        {
            handles.ForEach(i => i.Dispose());
        }
    }

    public static unsafe int Initialize(this HostFxr hostfxr, string runtimeConfigPath, SafeInitializeParameters? parameters, out HostContext context)
    {
        List<MemoryHandle> handles = new(2);
        try
        {
            var lRuntimeConfigPath = HostFxr.Encoding.GetByteCount(runtimeConfigPath);
            Span<byte> szRuntimeConfigPath = new byte[lRuntimeConfigPath + HostFxr.CharSize];
            HostFxr.Encoding.GetBytes(runtimeConfigPath, szRuntimeConfigPath);

            InitializeParameters* param = null;
            if (parameters is not null)
            {
                var chp = HostFxr.Encoding.GetByteCount(parameters.HostPath);
                byte[] szhp = new byte[chp + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(parameters.HostPath, szhp);
                var hhp = szhp.AsMemory().Pin();
                handles.Add(hhp);

                var cdr = HostFxr.Encoding.GetByteCount(parameters.DotnetRoot);
                byte[] szdr = new byte[cdr + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(parameters.DotnetRoot, szdr);
                var hdr = szdr.AsMemory().Pin();
                handles.Add(hdr);

                InitializeParameters tmp = new()
                {
                    Size = (nuint)sizeof(InitializeParameters),
                    HostPath = (byte*)hhp.Pointer,
                    DotnetRoot = (byte*)hdr.Pointer,
                };
                param = &tmp;
            }
            fixed (byte* lpszRuntimeConfigPath = szRuntimeConfigPath)
            {
                var result = hostfxr.InitializeForRuntimeConfig(lpszRuntimeConfigPath, param, out var hContext);
                context = new(hostfxr, hContext);
                return result;
            }
        }
        finally
        {
            handles.ForEach(i => i.Dispose());
        }
    }


    //public static unsafe void GetDotnetEnvironmentInfoResult(this HostFxr hostfxr, in DotnetEnvironmentInfo info, nint resultContext)
    //{
    //    fixed (DotnetEnvironmentInfo* lpInfo = &info)
    //        hostfxr.GetDotnetEnvironmentInfoResult(lpInfo, (void*)resultContext);
    //}

    public static unsafe int Main(this HostFxr hostfxr, string[] args)
    {
        MemoryHandle[] handles = new MemoryHandle[args.Length];
        try
        {
            byte*[] argv = new byte*[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                var str = args[i];
                var lstr = HostFxr.Encoding.GetByteCount(str);
                byte[] sz = new byte[lstr + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(str, sz);
                handles[i] = sz.AsMemory().Pin();
                argv[i] = (byte*)handles[i].Pointer;
            }

            fixed (byte** ptr = argv)
                return hostfxr.Main(args.Length, ptr);
        }
        finally
        {
            foreach (var item in handles)
            {
                item.Dispose();
            }
        }
    }
    public static unsafe int MainBundleStartupinfo(this HostFxr hostfxr, string[] args, string hostPath, string dotnetRoot, string appPath, long bundleHeaderOffset)
    {
        MemoryHandle[] handles = new MemoryHandle[args.Length];
        try
        {
            byte*[] argv = new byte*[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                var str = args[i];
                var lstr = HostFxr.Encoding.GetByteCount(str);
                byte[] sz = new byte[lstr + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(str, sz);
                handles[i] = sz.AsMemory().Pin();
                argv[i] = (byte*)handles[i].Pointer;
            }

            var cHostPath = HostFxr.Encoding.GetByteCount(hostPath);
            Span<byte> szHostPath = new byte[cHostPath + HostFxr.CharSize];
            HostFxr.Encoding.GetBytes(hostPath, szHostPath);

            var cDotnetRoot = HostFxr.Encoding.GetByteCount(dotnetRoot);
            Span<byte> szDotnetRoot = new byte[cDotnetRoot + HostFxr.CharSize];
            HostFxr.Encoding.GetBytes(dotnetRoot, szDotnetRoot);

            var cAppPath = HostFxr.Encoding.GetByteCount(appPath);
            Span<byte> szAppPath = new byte[cAppPath + HostFxr.CharSize];
            HostFxr.Encoding.GetBytes(appPath, szAppPath);

            fixed (byte** ptr = argv)
            fixed (byte* lpszHostPath = szHostPath)
            fixed (byte* lpszDotnetRoot = szDotnetRoot)
            fixed (byte* lpszAppPath = szAppPath)
                return hostfxr.MainBundleStartupinfo(args.Length, ptr, lpszHostPath, lpszDotnetRoot, lpszAppPath, bundleHeaderOffset);
        }
        finally
        {
            foreach (var item in handles)
            {
                item.Dispose();
            }
        }
    }
    public static unsafe int MainStartupinfo(this HostFxr hostfxr, string[] args, string hostPath, string dotnetRoot, string appPath)
    {
        MemoryHandle[] handles = new MemoryHandle[args.Length];
        try
        {
            byte*[] argv = new byte*[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                var str = args[i];
                var lstr = HostFxr.Encoding.GetByteCount(str);
                byte[] sz = new byte[lstr + HostFxr.CharSize];
                HostFxr.Encoding.GetBytes(str, sz);
                handles[i] = sz.AsMemory().Pin();
                argv[i] = (byte*)handles[i].Pointer;
            }

            var cHostPath = HostFxr.Encoding.GetByteCount(hostPath);
            Span<byte> szHostPath = new byte[cHostPath + HostFxr.CharSize];
            HostFxr.Encoding.GetBytes(hostPath, szHostPath);

            var cDotnetRoot = HostFxr.Encoding.GetByteCount(dotnetRoot);
            Span<byte> szDotnetRoot = new byte[cDotnetRoot + HostFxr.CharSize];
            HostFxr.Encoding.GetBytes(dotnetRoot, szDotnetRoot);

            var cAppPath = HostFxr.Encoding.GetByteCount(appPath);
            Span<byte> szAppPath = new byte[cAppPath + HostFxr.CharSize];
            HostFxr.Encoding.GetBytes(appPath, szAppPath);

            fixed (byte** ptr = argv)
            fixed (byte* lpszHostPath = szHostPath)
            fixed (byte* lpszDotnetRoot = szDotnetRoot)
            fixed (byte* lpszAppPath = szAppPath)
                return hostfxr.MainStartupinfo(args.Length, ptr, lpszHostPath, lpszDotnetRoot, lpszAppPath);
        }
        finally
        {
            foreach (var item in handles)
            {
                item.Dispose();
            }
        }

    }

    //public static unsafe int ResolveFrameworksForRuntimeConfig(this HostFxr hostfxr, string runtimeConfigPath, SafeInitializeParameters? parameters, Action<SafeResolveFrameworksResult, nint>? callback, nint resultContext)
    //{
    //    List<MemoryHandle> handles = new(2);
    //    try
    //    {
    //        var lRuntimeConfigPath = HostFxr.Encoding.GetByteCount(runtimeConfigPath);
    //        Span<byte> szRuntimeConfigPath = new byte[lRuntimeConfigPath + HostFxr.CharSize];
    //        HostFxr.Encoding.GetBytes(runtimeConfigPath, szRuntimeConfigPath);

    //        InitializeParameters* param = null;
    //        if (parameters is not null)
    //        {
    //            var chp = HostFxr.Encoding.GetByteCount(parameters.HostPath);
    //            byte[] szhp = new byte[chp + HostFxr.CharSize];
    //            HostFxr.Encoding.GetBytes(parameters.HostPath, szhp);
    //            var hhp = szhp.AsMemory().Pin();
    //            handles.Add(hhp);

    //            var cdr = HostFxr.Encoding.GetByteCount(parameters.DotnetRoot);
    //            byte[] szdr = new byte[cdr + HostFxr.CharSize];
    //            HostFxr.Encoding.GetBytes(parameters.DotnetRoot, szdr);
    //            var hdr = szdr.AsMemory().Pin();
    //            handles.Add(hdr);

    //            InitializeParameters tmp = new()
    //            {
    //                Size = (nuint)sizeof(InitializeParameters),
    //                HostPath = (byte*)hhp.Pointer,
    //                DotnetRoot = (byte*)hdr.Pointer,
    //            };
    //            param = &tmp;
    //        }
    //        fixed (byte* lpszRuntimeConfigPath = szRuntimeConfigPath)
    //            return hostfxr.ResolveFrameworksForRuntimeConfig(lpszRuntimeConfigPath, param, null, (void*)resultContext);
    //    }
    //    finally
    //    {
    //        handles.ForEach(i => i.Dispose());
    //    }
    //}

    public static unsafe void SetErrorWriter(this HostFxr hostfxr, delegate*<string?, void> errorWriter)
    {
        delegate* unmanaged[Cdecl]<byte*, void> writer = (delegate* unmanaged[Cdecl]<byte*, void>)
            Marshal.GetFunctionPointerForDelegate<ErrorWriter>(
                lpszMessage => errorWriter(HostFxr.PtrToString((nint)lpszMessage)));

        hostfxr.SetErrorWriter(writer);
    }
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
file unsafe delegate void ErrorWriter(byte* message);