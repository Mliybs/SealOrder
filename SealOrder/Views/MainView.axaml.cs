using Avalonia.Controls;

namespace SealOrder.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "114"));

        using var ouo = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "114","ouo.txt"));

        ouo.Write("114!");
    }
}