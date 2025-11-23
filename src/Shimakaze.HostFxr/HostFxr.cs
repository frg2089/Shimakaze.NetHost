using System.Runtime.InteropServices;
using System.Text;

namespace Shimakaze;

public sealed class HostFxr(nint handle) : SafeHandleZeroIsInvalid(handle)
{
    public const int Success = 0;
    public const int Success_HostAlreadyInitialized = 1;
    public const int Success_DifferentRuntimeProperties = 2;

    // hostfxr_get_available_sdks
    // hostfxr_get_native_search_directories
    // hostfxr_resolve_sdk
    // hostfxr_resolve_sdk2
    public new unsafe delegate* unmanaged[Cdecl]<nint, int> Close => field != null ? field : field = (delegate* unmanaged[Cdecl]<nint, int>)NativeLibrary.GetExport(handle, "hostfxr_close");
    public unsafe delegate* unmanaged[Cdecl]<DotnetEnvironmentInfo*, void*, void> GetDotnetEnvironmentInfoResult => field != null ? field : field = (delegate* unmanaged[Cdecl]<DotnetEnvironmentInfo*, void*, void>)NativeLibrary.GetExport(handle, "hostfxr_get_dotnet_environment_info");
    public unsafe delegate* unmanaged[Cdecl]<nint, DelegateType, out void**, int> GetRuntimeDelegate => field != null ? field : field = (delegate* unmanaged[Cdecl]<nint, DelegateType, out void**, int>)NativeLibrary.GetExport(handle, "hostfxr_get_runtime_delegate");
    public unsafe delegate* unmanaged[Cdecl]<nint, ref nuint, out byte**, out byte**, int> GetRuntimeProperties => field != null ? field : field = (delegate* unmanaged[Cdecl]<nint, ref nuint, out byte**, out byte**, int>)NativeLibrary.GetExport(handle, "hostfxr_get_runtime_properties");
    public unsafe delegate* unmanaged[Cdecl]<nint, byte*, out byte**, int> GetRuntimePropertyValue => field != null ? field : field = (delegate* unmanaged[Cdecl]<nint, byte*, out byte**, int>)NativeLibrary.GetExport(handle, "hostfxr_get_runtime_property_value");
    public unsafe delegate* unmanaged[Cdecl]<int, byte**, InitializeParameters*, out nint, int> InitializeForDotnetCommandLine => field != null ? field : field = (delegate* unmanaged[Cdecl]<int, byte**, InitializeParameters*, out nint, int>)NativeLibrary.GetExport(handle, "hostfxr_initialize_for_dotnet_command_line");
    public unsafe delegate* unmanaged[Cdecl]<byte*, InitializeParameters*, out nint, int> InitializeForRuntimeConfig => field != null ? field : field = (delegate* unmanaged[Cdecl]<byte*, InitializeParameters*, out nint, int>)NativeLibrary.GetExport(handle, "hostfxr_initialize_for_runtime_config");
    public unsafe delegate* unmanaged[Cdecl]<int, byte**, int> Main => field != null ? field : field = (delegate* unmanaged[Cdecl]<int, byte**, int>)NativeLibrary.GetExport(handle, "hostfxr_main");
    public unsafe delegate* unmanaged[Cdecl]<int, byte**, byte*, byte*, byte*, long, int> MainBundleStartupinfo => field != null ? field : field = (delegate* unmanaged[Cdecl]<int, byte**, byte*, byte*, byte*, long, int>)NativeLibrary.GetExport(handle, "hostfxr_main_bundle_startupinfo");
    public unsafe delegate* unmanaged[Cdecl]<int, byte**, byte*, byte*, byte*, int> MainStartupinfo => field != null ? field : field = (delegate* unmanaged[Cdecl]<int, byte**, byte*, byte*, byte*, int>)NativeLibrary.GetExport(handle, "hostfxr_main_startupinfo");
    public unsafe delegate* unmanaged[Cdecl]<byte*, /*opt*/ InitializeParameters*, /*opt*/ delegate* unmanaged[Cdecl]<ResolveFrameworksResult*, void*, void>, /*opt*/ void*, int> ResolveFrameworksForRuntimeConfig => field != null ? field : field = (delegate* unmanaged[Cdecl]<byte*, InitializeParameters*, delegate* unmanaged[Cdecl]<ResolveFrameworksResult*, void*, void>, void*, int>)NativeLibrary.GetExport(handle, "hostfxr_resolve_frameworks_for_runtime_config");
    public unsafe delegate* unmanaged[Cdecl]<nint, int> RunApp => field != null ? field : field = (delegate* unmanaged[Cdecl]<nint, int>)NativeLibrary.GetExport(handle, "hostfxr_run_app");
    public unsafe delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<byte*, void>, delegate* unmanaged[Cdecl]<byte*, void>> SetErrorWriter => field != null ? field : field = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<byte*, void>, delegate* unmanaged[Cdecl]<byte*, void>>)NativeLibrary.GetExport(handle, "hostfxr_set_error_writer");
    public unsafe delegate* unmanaged[Cdecl]<nint, byte*, byte*, int> SetRuntimePropertyValue => field != null ? field : field = (delegate* unmanaged[Cdecl]<nint, byte*, byte*, int>)NativeLibrary.GetExport(handle, "hostfxr_set_runtime_property_value");

    internal static Encoding Encoding => field ??=
#if NETFRAMEWORK
        Encoding.Unicode;
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? Encoding.Unicode
            : Encoding.UTF8;
#endif

    internal static string? PtrToString(nint ptr)
    {
#if !NETFRAMEWORK
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
        {
            return Marshal.PtrToStringUni(ptr);
        }
#if !NETFRAMEWORK
        else
        {
#if NETSTANDARD2_0
            return Marshal.PtrToStringAnsi(ptr);
#else
            return Marshal.PtrToStringUTF8(ptr);
#endif
        }
#endif
    }

    internal static readonly int CharSize =
#if NETFRAMEWORK
        sizeof(char);
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? sizeof(char)
            : sizeof(byte);
#endif

    public HostFxr(string hostfxrPath)
        : this(NativeLibrary.Load(hostfxrPath))
    {
    }

    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        NativeLibrary.Free(handle);
        return true;
    }
}
