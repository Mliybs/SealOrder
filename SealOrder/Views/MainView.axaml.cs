using Avalonia.Controls;

namespace SealOrder.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Know(object sender, RoutedEventArgs e)
    {
        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "114"));

        using var ouo = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "114","ouo.txt"));

        ouo.Write("114!");

        MessageBoxManager.GetMessageBoxStandard(string.Empty, AppDomain.CurrentDomain.BaseDirectory).ShowAsync();
    }
}