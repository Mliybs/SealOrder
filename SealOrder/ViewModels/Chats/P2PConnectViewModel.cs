namespace SealOrder.ViewModels;

public class P2PConnectViewModel : ViewModelBase
{
    public P2PConnectViewModel()
    {
        ClientMode = ReactiveCommand.Create(() => {}, this.WhenAnyValue(x => x.InputIP, x =>
        {
            var array = x.Split(' ');
            if (array.Length != 2) return false;
            if (IPAddress.TryParse(array[0], out _) && int.TryParse(array[1], out int port) && port is >= 0 and <= 65535) return true;
            return false;
        }));
    }

    private string inputIP = string.Empty;

    public string InputIP
    {
        get => inputIP;
        set => this.RaiseAndSetIfChanged(ref inputIP, value);
    }

    public required ReactiveCommand<Unit, Unit> GetIP { get; init; }

    public ReactiveCommand<Unit, Unit> ServerMode { get; } = ReactiveCommand.Create(() => {});

    public ReactiveCommand<Unit, Unit> ClientMode { get; }
}
