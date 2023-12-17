
namespace SealOrder.Internals;

public class NativePlatformHandle: NativeControlHost
{
    public IPlatformHandle Implementation { get; }

    public NativePlatformHandle(IPlatformHandle handle) => Implementation = handle;

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent) => Implementation;
}