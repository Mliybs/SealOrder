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
        Directory.CreateDirectory(Path.Combine(LocalDirectory, "114"));

        using var ouo = new StreamWriter(Path.Combine(LocalDirectory, "114","ouo.txt"));

        ouo.Write("114!");

        MessageBoxManager.GetMessageBoxStandard(string.Empty, LocalDirectory).ShowAsync();
    }

    private void Get(object sender, RoutedEventArgs e)
    {
        MessageBoxManager.GetMessageBoxStandard(string.Empty, directory ?? "null").ShowAsync();
    }

    private void Raise(object sender, RoutedEventArgs e) => MessageBoxManager.GetMessageBoxStandard(string.Empty, except ?? "nope").ShowAsync();
}