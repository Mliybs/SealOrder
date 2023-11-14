
namespace SealOrder.Android;

public class ChatActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var view = new AvaloniaView(this)
        {
            Content = new ChatUsers()
        };

        SetContentView(view);
    }
}