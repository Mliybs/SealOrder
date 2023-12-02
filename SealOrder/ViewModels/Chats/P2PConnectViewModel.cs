namespace SealOrder.ViewModels;

public class P2PConnectViewModel : ViewModelBase
{
    public required ReactiveCommand<Unit, Unit> GetIP { get; init; }
}
