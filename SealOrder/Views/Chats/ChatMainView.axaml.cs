using System.Buffers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class ChatMainView : UserControl
{
    public event Func<byte[], Task<int>>? ToSend;

    private bool notPressed = true;

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
        Viewer.PointerPressed += (sender, e) => notPressed = false;
        Viewer.PointerReleased += (sender, e) => notPressed = true;
    }

    private void Hold(object sender, HoldingRoutedEventArgs e)
    {
        notPressed = !(e.HoldingState == HoldingState.Started);
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
        if (notPressed) Viewer.ScrollToEnd();
    }

    private void Send(object sender, RoutedEventArgs e)
    {
        if (ToSend is null) return;
        var text = Input.Text;
        if (text is null) return;
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
        if (notPressed) Viewer.ScrollToEnd();
    }
}