namespace Shimakaze;

internal static class HostFxrExtensions
{
    public static unsafe int Initialize(this HostFxr hostfxr, TString[] args, InitializeParameters? parameters, out HostContext context)
    {
        byte*[] argv = new byte*[args.Length];

        for (int i = 0; i < args.Length; i++)
            argv[i] = args[i];

        InitializeParametersStruct* param = null;
        parameters?.ToStruct(ref *param);
        fixed (byte** ptr = argv)
        {
            var result = hostfxr.InitializeForDotnetCommandLine(args.Length, ptr, param, out nint hContext);

            context = new(hostfxr, hContext);
            return result;
        }
    }

    public static unsafe int Initialize(this HostFxr hostfxr, TString runtime_config_path, InitializeParameters? parameters, out HostContext context)
    {
        InitializeParametersStruct* param = null;
        parameters?.ToStruct(ref *param);
        var result = hostfxr.InitializeForRuntimeConfig(runtime_config_path, param, out nint hContext);

        context = new(hostfxr, hContext);
        return result;
    }
}