namespace Shimakaze;

public unsafe struct DotnetEnvironmentFrameworkInfo()
{
    public nuint Size = unchecked((nuint)(sizeof(nint) * 4));
    public byte* Name = null;
    public byte* Version = null;
    public byte* Path = null;
}
