using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SealOrder.Views;

namespace SealOrder;

public partial class UserView : UserControl
{
    public UserView()
    {
        InitializeComponent();
    }

    private async void Read(object sender, RoutedEventArgs e)
    {
        if (Parent is TopLevel control)
        {
            var picker = await control.StorageProvider.OpenFilePickerAsync(new()
            {
                SuggestedStartLocation = await control.StorageProvider.TryGetFolderFromPathAsync("/storage/emulated/0/Android/data/com.tencent.mobileqq/Tencent/QQfile_recv")
            });
        }
    }
}