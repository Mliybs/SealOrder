using System.Buffers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class ChatMainView : UserControl
{
    public event Func<byte[], Task<int>>? ToSend;

    public ChatMainView()
    {
        InitializeComponent();
    }

    public ChatMainView(out BytesDelegate received, Func<byte[], Task<int>> send)
    {
        InitializeComponent();

        received = Received;

        ToSend += send;
    }

    private void OnLoad(object sender, RoutedEventArgs e)
    {
        SendButton.Bind(IsVisibleProperty, Input.WhenAnyValue(x => x.Text, x => !string.IsNullOrEmpty(x)));
        Viewer.ScrollChanged += (sender, e) => { if (e.OffsetDelta.Y == 0 && e.ExtentDelta.Y != 0) Viewer.ScrollToEnd(); Input.Text = e.OffsetDelta.Y.ToString(); };
    }

    private void Received(in ReadOnlySequence<byte> bytes)
    {
        Messages.Children.Add(new Border()
        {
            Child = new TextBlock()
            {
                Text = Encoding.UTF8.GetString(in bytes)
            },
            Classes = { "You" }
        });
    }

    private void Send(object sender, RoutedEventArgs e)
    {
        if (ToSend is null) return;
        var text = Input.Text;
        if (string.IsNullOrEmpty(text)) return;
        Input.Text = null;
        Messages.Children.Add(new Border()
        {
            Child = new TextBlock()
            {
                Text = text
            },
            Classes = { "Me" }
        });
        ToSend.Invoke(Encoding.UTF8.GetBytes(text));
    }
}
