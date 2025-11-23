namespace Shimakaze;

public unsafe struct DotnetEnvironmentInfo()
{
    public nuint Size = unchecked((nuint)(sizeof(nint) * 7));

    public byte* HostfxrVersion = null;
    public byte* HostfxrCommitHash = null;

    public nint SdkCount = 0;
    public DotnetEnvironmentSdkInfo* Sdks = null;

    public nint FrameworkCount = 0;
    public DotnetEnvironmentFrameworkInfo* Frameworks = null;
}
