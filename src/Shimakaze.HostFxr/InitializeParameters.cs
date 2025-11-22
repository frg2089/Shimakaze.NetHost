namespace Shimakaze;

internal struct InitializeParameters
{
    public nuint Size;
    public unsafe char* HostPath;
    public unsafe char* DotnetRoot;
}