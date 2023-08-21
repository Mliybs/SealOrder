using Avalonia.Controls;

namespace SealOrder.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Change(object sender, RoutedEventArgs e)
    {
        if (Parent is ContentControl control)
        {
            control.Content = new UserView();
        }
    }
}