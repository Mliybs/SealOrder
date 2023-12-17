using Android.Views.InputMethods;

namespace SealOrder.Android;

[Activity(
    Label = "SealOrder",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode,
    WindowSoftInputMode = SoftInput.AdjustResize)]
public class ChatActivity : AvaloniaMainActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var view = new ChatMainView(out var received, async x => await Connect.Socket.SendAsync(x))
        {
            Handle = new(new AndroidViewControlHandle(new global::Android.Widget.Button(this) { Text = "WCNM" }))
        };

        if (Intent is not null) view.Loaded += async (sender, e) =>
        {
            var intent = Intent;
            switch (intent.GetIntExtra("mode", 0))
            {
                case 0: throw new InvalidOperationException("没有启动参数！");
                case 1: //客户端模式
                    var input = intent.GetStringExtra("input") ?? throw new ArgumentNullException("input", "没有输入参数！");
                    var array = input.Split(' ');
                    if (array.Length != 2) throw new ArgumentException("输入参数错误！");
                    if (!(IPAddress.TryParse(array[0], out _) && int.TryParse(array[1], out var port) && port is >= 0 and <= 65535))
                        throw new ArgumentException("输入参数错误！");
                    try
                    {
                        await Connect.AsClient(array[0], port);
                    }
                    catch
                    {
                        Toast.MakeText(this, "连接失败！", ToastLength.Short)?.Show();
                    }
                    break;
                case 2: //服务端模式
                    try{
                    var ip = intent.GetStringExtra("ip") ?? throw new ArgumentNullException("ip", "没有输入参数！");
                    Connect.AsServer();
                    _ = MessageBoxManager.GetMessageBoxStandard(string.Empty, $"您的服务器连接地址为：\n{ip} {Connect.Port}").ShowAsPopupAsync(view);
                    _ = TopLevel.GetTopLevel(view)?.Clipboard?.SetTextAsync($"{ip} {Connect.Port}");
                    Toast.MakeText(this, "已复制到剪贴板", ToastLength.Short)?.Show();
                    }
                    catch(Exception exc){Toast.MakeText(this, $"{exc.GetType()}\n{exc.Message}", ToastLength.Short)?.Show();}
                    break;
            }

            Connect.Received(received);
        };

        SetContentView(new AvaloniaView(this)
        {
            Content = view
        });
    }

    public P2PConnect Connect { get; } = new();
}
