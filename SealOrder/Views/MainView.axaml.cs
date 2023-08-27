using Avalonia.Controls;

namespace SealOrder.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        Directory.CreateDirectory("114");

        using var ouo = new StreamWriter("ouo.txt");

        ouo.Write("114!");
    }
}