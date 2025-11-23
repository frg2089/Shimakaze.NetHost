namespace Shimakaze;

internal sealed record class InitializeParameters(TString? HostPath, TString? DotnetRoot)
{
    public unsafe void ToStruct(ref InitializeParametersStruct initializeParameters)
    {
        initializeParameters = new()
        {
            Size = unchecked((nuint)sizeof(InitializeParametersStruct)),
            HostPath = HostPath,
            DotnetRoot = DotnetRoot,
        };
    }
}