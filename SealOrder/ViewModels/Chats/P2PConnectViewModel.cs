namespace SealOrder.ViewModels;

public class P2PConnectViewModel : ViewModelBase
{
    public P2PConnectViewModel(out IObservable<bool> isInputValid, out IObservable<bool> isIpValid, out Action<string> modify)
    {
        isInputValid = this.WhenAnyValue(x => x.InputIP, x =>
        {
            var array = x.Split(' ');
            if (array.Length != 2) return false;
            if (IPAddress.TryParse(array[0], out _) && int.TryParse(array[1], out int port) && port is >= 0 and <= 65535) return true;
            return false;
        });

        isIpValid = this.WhenAnyValue(x => x.PublicIP, x => IPAddress.TryParse(x, out _));

        modify = x => PublicIP = x;
    }

    private string inputIP = string.Empty;

    private string publicIP = string.Empty;

    public string InputIP
    {
        get => inputIP;
        set => this.RaiseAndSetIfChanged(ref inputIP, value);
    }

    public string PublicIP
    {
        get => publicIP;
        set => this.RaiseAndSetIfChanged(ref publicIP, value);
    }

    public required ReactiveCommand<Unit, Unit> GetIP { get; init; }

    public required ReactiveCommand<string, Unit> ServerMode { get; init; }

    public required ReactiveCommand<string, Unit> ClientMode { get; init; }
}
