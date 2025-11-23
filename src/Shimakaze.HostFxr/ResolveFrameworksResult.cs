namespace Shimakaze;

public unsafe struct ResolveFrameworksResult()
{
    public nuint Size = unchecked((nuint)(sizeof(nint) * 5));

    public nuint ResolvedCount = 0;
    public FrameworkResult* ResolvedFrameworks = null;

    public nuint UnresolvedCount = 0;
    public FrameworkResult* UnresolvedFrameworks = null;
}

public sealed record class SafeResolveFrameworksResult(SafeFrameworkResult[] ResolvedFrameworks, SafeFrameworkResult[] UnresolvedFrameworks);