namespace Shimakaze;

public unsafe struct FrameworkResult()
{
    public nuint Size = unchecked((nuint)(sizeof(nint) * 5));
    public byte* Name = null;
    public byte* RequestedVersion = null;
    public byte* ResolvedVersion = null;
    public byte* ResolvedPath = null;
}

public sealed record class SafeFrameworkResult(string Name, string RequestedVersion, string ResolvedVersion, string ResolvedPath);