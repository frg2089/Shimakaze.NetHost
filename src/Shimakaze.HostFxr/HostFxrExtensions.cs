namespace Shimakaze;

internal static class HostFxrExtensions
{
    extension(HostFxr hostfxr)
    {
        public unsafe int Initialize(string[] args, InitializeParameters? parameters, out HostContext context)
        {
            byte*[] argv = new byte*[args.Length];

            for (int i = 0; i < args.Length; i++)
                argv[i] = (TString)args[i];

            InitializeParametersStruct* param = null;
            parameters?.ToStruct(ref *param);
            fixed (byte** ptr = argv)
            {
                var result = hostfxr.InitializeForDotnetCommandLine(args.Length, ptr, param, out nint hContext);

                context = new(hostfxr, hContext);
                return result;
            }
        }

        public unsafe int Initialize(string runtime_config_path, InitializeParameters? parameters, out HostContext context)
        {
            InitializeParametersStruct* param = null;
            parameters?.ToStruct(ref *param);
            var result = hostfxr.InitializeForRuntimeConfig((TString)runtime_config_path, param, out nint hContext);

            context = new(hostfxr, hContext);
            return result;
        }
    }
}