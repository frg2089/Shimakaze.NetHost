using System.Runtime.InteropServices;

namespace Shimakaze;

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate int hostfxr_main_fn(int argc, string[] argv);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate int hostfxr_main_startupinfo_fn(int argc, string[] argv, string host_path, string dotnet_root, string app_path);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate int hostfxr_main_bundle_startupinfo_fn(int argc, string[] argv, string host_path, string dotnet_root, string app_path, long bundle_header_offset);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate void hostfxr_error_writer_fn(string message);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate hostfxr_error_writer_fn hostfxr_set_error_writer_fn(hostfxr_error_writer_fn? error_writer);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate int hostfxr_get_runtime_property_value_fn(nint host_context_handle, string name, out string value);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate int hostfxr_set_runtime_property_value_fn(nint host_context_handle, string name, string value);

[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
internal delegate int hostfxr_get_runtime_properties_fn(nint host_context_handle, ref nuint count, out string[] keys, out string[] values);

[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
internal delegate int load_assembly_and_get_function_pointer_fn(
    string assembly_path,
    string type_name,
    string method_name,
    string? delegate_type_name,
    nint reserved,
    out nint @delegate);
