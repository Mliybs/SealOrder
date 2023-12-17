using System.Buffers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class ChatMainView : UserControl
{
    public event Func<byte[], Task<int>>? ToSend;

    public Func<string, Control, Control>? Handle { get; init; }

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
        var control = Handle?.Invoke("Viewer", new Messages()) ?? new ScrollViewer() { Name = "Viewer" };
        Panel.Children.Add(control);
    }

    private void Received(in ReadOnlySequence<byte> bytes)
    {
        /*
        Messages.Children.Add(new Border()
        {
            Child = new TextBlock()
            {
                Text = Encoding.UTF8.GetString(in bytes)
            },
            Classes = { "You" }
        });
        */
    }

    private void Send(object sender, RoutedEventArgs e)
    {
        if (this.FindControl<StackPanel>("Messages") is StackPanel panel)
            panel.Children.Add(new Border()
            {
                Child = new TextBlock()
                {
                    Text = Input.Text
                },
                Classes = { "Me" }
            });

        else
        {
            Input.Text = "null";
        }
        /*
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
        */
    }
}
