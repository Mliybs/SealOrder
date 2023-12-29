using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class P2PConnectView : UserControl
{
    public P2PConnectView()
    {
        InitializeComponent();
    }

    private void Focused(object sender, GotFocusEventArgs e)
    {
        var button = (Button)sender;
        button.Command?.Execute(button.CommandParameter);
    }
}