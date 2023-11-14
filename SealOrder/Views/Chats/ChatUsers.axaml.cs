using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class ChatUsers : UserControl
{
    public ChatUsers()
    {
        InitializeComponent();
    }

    public ChatUsers(string id, string user, bool online)
    {
        InitializeComponent();

        ID = id;

        Console.WriteLine(user);

        ((Content as Grid)!.Children[1] as TextBlock)!.Text = user;

        Online = online;
    }

    public string? ID { get; }

    public bool? Online { get; }

    private void Pressed(object sender, PointerReleasedEventArgs e) => Console.WriteLine(114);
}