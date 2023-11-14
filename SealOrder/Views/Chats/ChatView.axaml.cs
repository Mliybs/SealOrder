using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();

        // if (Content is ScrollViewer viewer && viewer.Content is StackPanel panel)
        // {
        //     socket.On("online_changed", x =>
        //     {
        //         var res = x.GetValue<JsonElement>();

        //         foreach (var item in x.GetValue<JsonElement>().GetProperty("users").EnumerateArray())
        //             Dispatcher.UIThread.Post(() => panel.Children.Add(new ChatUsers(item[0].ToString(), item[1].ToString(), item[2].GetBoolean())));
        //     });
            
        //     socket.On("receive", x =>
        //     {
        //         Console.WriteLine(x.GetValue<string>(2).ToString());
        //     });

        //     socket.ConnectAsync();
        // }
    }

    private void Send(object sender, RoutedEventArgs e) => ToNotify?.Invoke();
}