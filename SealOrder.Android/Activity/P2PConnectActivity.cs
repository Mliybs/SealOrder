namespace SealOrder.Android;

[Activity(
    Label = "SealOrder",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class P2PConnectActivity : AvaloniaMainActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var view = new P2PConnectView();

        view.DataContext = new P2PConnectViewModel(async () =>
        {
            var res = await Client.GetAsync("https://service.mliybs.top/ip");

            if (!res.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "牛魔的报错了", ToastLength.Short)?.Show();
                return;
            }

            var ip = await res.Content.ReadAsStringAsync();

            var interfaces = Java.Net.NetworkInterface.NetworkInterfaces;

            while (interfaces?.HasMoreElements ?? false)
            {
                var items = (interfaces.NextElement() as Java.Net.NetworkInterface)?.InetAddresses;

                while (items?.HasMoreElements ?? false)
                {
                    var address = (Java.Net.InetAddress)items.NextElement()!;
                    if (ip == address.HostAddress)
                    {
                        _ = MessageBoxManager.GetMessageBoxStandard(string.Empty, $"您的可用公网IP为：\n{ip}").ShowAsPopupAsync(view);
                        _ = TopLevel.GetTopLevel(view)?.Clipboard?.SetTextAsync(ip);
                        Toast.MakeText(this, "已复制到剪贴板", ToastLength.Short)?.Show();
                        return;
                    }
                }
            }

            _ = MessageBoxManager.GetMessageBoxStandard(string.Empty, $"您没有可用的公网IP！").ShowAsPopupAsync(view);
        });

        SetContentView(new AvaloniaView(this)
        {
            Content = view
        });
    }
}