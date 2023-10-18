using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();

        // if (Content is StackPanel panel)
        // {
        //     socket.On("online_changed", x =>
        //     {
        //         var res = x.GetValue<JsonElement>();

        //         foreach (var item in x.GetValue<JsonElement>().GetProperty("users").EnumerateArray())
        //             Console.WriteLine(item.ToString());
        //     });
            
        //     socket.On("receive", x =>
        //     {
        //         Console.WriteLine(x.GetValue<string>(2).ToString());
        //     });

        //     socket.OnConnected += (sender, e) =>
        //     {
        //         Console.WriteLine("连上了WRYYYYYYYYYYY");
        //     };

        //     socket.ConnectAsync();
        // }
    }

    private void Send(object sender, RoutedEventArgs e) => ToNotify?.Invoke();
}