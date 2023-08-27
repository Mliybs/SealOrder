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
        Directory.CreateDirectory(Path.Combine(LocalCacheDirectory, "114"));

        using var ouo = new StreamWriter(Path.Combine(LocalCacheDirectory, "114", "ouo.txt"));

        ouo.Write("114!");

        MessageBoxManager.GetMessageBoxStandard(string.Empty, LocalCacheDirectory).ShowAsync();
    }

    private void GetCache(object sender, RoutedEventArgs e)
    {
        MessageBoxManager.GetMessageBoxStandard(string.Empty, LocalCacheDirectory).ShowAsync();
    }

    private void GetFile(object sender, RoutedEventArgs e)
    {
        MessageBoxManager.GetMessageBoxStandard(string.Empty, LocalFileDirectory).ShowAsync();
    }
}