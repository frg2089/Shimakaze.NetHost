namespace Shimakaze;

public unsafe struct DotnetEnvironmentSdkInfo()
{
    public nuint Size = unchecked((nuint)(sizeof(nint) * 3));
    public byte* Version = null;
    public byte* Path = null;
}
