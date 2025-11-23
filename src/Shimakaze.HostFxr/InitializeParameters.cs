namespace Shimakaze;

public unsafe struct InitializeParameters()
{
    public nuint Size = unchecked((nuint)(sizeof(nint) * 3));
    public byte* HostPath = null;
    public byte* DotnetRoot = null;
}
public sealed record class SafeInitializeParameters(string HostPath, string DotnetRoot);