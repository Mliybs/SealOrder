
namespace SealOrder.Android;

[Activity(
    Label = "原神",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class ChatActivity : AvaloniaMainActivity
{
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