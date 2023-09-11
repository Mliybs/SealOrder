using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;
using SealOrder.ViewModels;

namespace SealOrder.Views;

public partial class MenuBox : UserControl
{
    public MenuBox()
    {
        InitializeComponent();

        DataContext = new MenuBoxViewModel();
    }

    public MenuBox(List<string> list)
    {
        InitializeComponent();

        if (Content is Grid content)
            content.Children.RemoveAt(0);

        DataContext = new MenuBoxViewModel(list);
    }

    public MenuBox(IEnumerable<string> collection)
    {
        InitializeComponent();

        if (Content is Grid content)
            content.Children.RemoveAt(0);

        DataContext = new MenuBoxViewModel(collection);
    }

    public MenuBox(string hint, List<string> list)
    {
        InitializeComponent();

        DataContext = new MenuBoxViewModel(hint, list);
    }

    public MenuBox(string hint, IEnumerable<string> collection)
    {
        InitializeComponent();

        DataContext = new MenuBoxViewModel(hint, collection);
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        if ((Parent is MsBox.Avalonia.Controls.MsBoxCustomView view) && (view.DataContext is MsBox.Avalonia.ViewModels.MsBoxCustomViewModel model))
        {
            view.SetButtonResult(this.GetControl<ListBox>("List").SelectedItem as string);

            view.Close();
        }
    }
}