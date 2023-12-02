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

        SetContentView(new AvaloniaView(this)
        {
            Content = new P2PConnectView()
            {
                DataContext = new P2PConnectViewModel()
                {
                    GetIP = ReactiveCommand.CreateFromTask(async () =>
                    {
                        var res = await Client.GetAsync("https://service.mliybs.top/ip");

                        if (!res.IsSuccessStatusCode)
                        {
                            Toast.MakeText(this, "牛魔的报错了", ToastLength.Short)?.Show();

                            return;
                        }
                        string ip = await res.Content.ReadAsStringAsync();

                        var interfaces = Java.Net.NetworkInterface.NetworkInterfaces;

                        while (interfaces?.HasMoreElements ?? false)
                        {
                            var items = (interfaces.NextElement() as Java.Net.NetworkInterface)?.InetAddresses;

                            while (items?.HasMoreElements ?? false)
                            {
                                var address = (Java.Net.InetAddress)items.NextElement()!;
                                if (ip == address.HostAddress)
                                {
                                    Toast.MakeText(this, address.HostAddress, ToastLength.Short)?.Show();

                                    return;
                                }
                            }
                        }
                    })
                }
            }
        });
    }
}