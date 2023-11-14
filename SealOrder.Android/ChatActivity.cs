
namespace SealOrder.Android;

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