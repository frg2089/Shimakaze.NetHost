#if NETFRAMEWORK || NETSTANDARD
namespace System.Runtime.InteropServices;

internal static class NativeLibrary
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