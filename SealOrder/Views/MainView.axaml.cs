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

    private void GetFiles(object sender, RoutedEventArgs e)
    {
        try
        {
            var files = Directory.GetFiles(Path.Combine(LocalCacheDirectory, "../../com.tencent.mobileqq/Tencent/QQfile_recv"));

            var builder = new StringBuilder();

            for (var i = 0; i < files.Length; i++)
                builder.Append(files[i]);

            MessageBoxManager.GetMessageBoxStandard(string.Empty, builder.ToString()).ShowAsync();
        }
        catch (Exception exc)
        {
            MessageBoxManager.GetMessageBoxStandard(string.Empty, exc.Message).ShowAsync();
        }
    }

    private void GetMime(object sender, RoutedEventArgs e)
    {
        MessageBoxManager.GetMessageBoxStandard(string.Empty, Mime ?? "null").ShowAsync();
    }
}