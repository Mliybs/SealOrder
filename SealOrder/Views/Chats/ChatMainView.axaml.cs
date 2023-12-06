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

    private void Received(in ReadOnlySequence<byte> bytes)
    {
        Messages.Children.Add(new TextBlock()
        {
            Text = Encoding.UTF8.GetString(bytes)
        });
    }

    private void Send(object sender, RoutedEventArgs e)
    {
        if (ToSend is null) return;
        var text = Input.Text;
        if (text is null) return;
        ToSend.Invoke(Encoding.UTF8.GetBytes(text));
    }
}