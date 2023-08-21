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
        if (Parent is Window window)
        {
            window.Content = new UserView();
        }

        MessageBoxManager.GetMessageBoxStandard(string.Empty, Parent.GetType().ToString()).ShowAsync();
    }
}