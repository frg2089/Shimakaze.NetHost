namespace Shimakaze;

internal ref struct InitializeParametersStruct
{
    public nuint Size;
    public unsafe byte* HostPath;
    public unsafe byte* DotnetRoot;
}
