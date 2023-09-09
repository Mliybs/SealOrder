using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SealOrder.Views;

public partial class InputBox : UserControl
{
    public InputBox()
    {
        InitializeComponent();
    }

    public InputBox(string watermark)
    {
        InitializeComponent();

        this.GetControl<TextBox>("Input").Watermark = watermark;
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        if ((Parent is MsBox.Avalonia.Controls.MsBoxCustomView view) && (view.DataContext is MsBox.Avalonia.ViewModels.MsBoxCustomViewModel model))
        {
            view.SetButtonResult(this.GetControl<TextBox>("Input").Text);

            model.ButtonClick(string.Empty);
        }
    }
}