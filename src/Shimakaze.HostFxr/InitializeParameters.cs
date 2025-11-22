namespace Shimakaze;

internal sealed record class InitializeParameters(string? HostPath, string? DotnetRoot)
{
    public unsafe void ToStruct(ref InitializeParametersStruct initializeParameters)
    {
        initializeParameters = new()
        {
            Size = unchecked((nuint)sizeof(InitializeParametersStruct)),
            HostPath = (TString)HostPath,
            DotnetRoot = (TString)DotnetRoot,
        };
    }
}