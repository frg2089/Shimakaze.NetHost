using System.Runtime.InteropServices;

namespace Shimakaze;

/// <inheritdoc/>
public abstract class SafeHandleZeroIsInvalid(nint handle) : SafeHandle(handle, true)
{
    /// <inheritdoc/>
#if NET7_0_OR_GREATER
    public override bool IsInvalid => handle is 0;
#else
    public override bool IsInvalid => handle is (nint)0; 
#endif
}
