using System.Text.Json.Nodes;
using Avalonia.Controls;

namespace SealOrder.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void Load(object sender, RoutedEventArgs e)
    {
        if (LoadedFile is not null)
            try
            {
                if (TopLevel.GetTopLevel(this) is not null and var level)
                {
                    var bytes = AESDecrypt(LoadedFile.Value.Bytes(), JsonDocument.Parse(Access!).RootElement.GetProperty("key").GetString()!, JsonDocument.Parse(Access!).RootElement.GetProperty("iv").GetString()!);

                    var save = await level.StorageProvider.SaveFilePickerAsync(new()
                    {
                        SuggestedFileName = LoadedFile.Value.Path[(LoadedFile.Value.Path.LastIndexOf('/') + 1)..].Replace(".mdf", ".pdf")
                    });

                    if (save is not null)
                    {
                        await File.WriteAllBytesAsync(save.Path.AbsolutePath, bytes);

                        await MessageBoxManager.GetMessageBoxStandard(string.Empty, "保存成功").ShowAsync();
                    }
                }

                else
                    await MessageBoxManager.GetMessageBoxStandard(string.Empty, "获取顶级控件失败！").ShowAsync();
            }
            catch (Exception exc)
            {
                await MessageBoxManager.GetMessageBoxStandard(string.Empty, exc.Message).ShowAsync();
            }
    }

    private void Loading(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox box)
        {
            if (box.Text is not null)
            {
                var access = new JsonObject();

                foreach (var item in box.Text.Split(' '))
                {
                    switch (item.Split('：'))
                    {
                        case ["等级", var value]:

                            access["access"] = value;
                        
                            break;
                            
                        case ["密钥", var value]:

                            access["key"] = value;
                        
                            break;
                            
                        case ["向量", var value]:

                            access["iv"] = value;
                        
                            break;

                        default:

                            MessageBoxManager.GetMessageBoxStandard(string.Empty, "通行等级不合法！").ShowAsync();

                            break;
                    }
                }

                File.WriteAllTextAsync(Path.Combine(DataDirectory, "access.json"), access.ToString());

                MessageBoxManager.GetMessageBoxStandard(string.Empty, "导入成功！").ShowAsync();

                box.Text = null;
            }
        }
    }

    private async void Encrypt(object sender, RoutedEventArgs e)
    {
        try
        {
            if (TopLevel.GetTopLevel(this) is not null and var level)
            {
                var picker = await level.StorageProvider.OpenFilePickerAsync(new()
                {
                    AllowMultiple = false
                });

                if (picker?.SingleOrDefault() is not null and var file)
                {
                    var bytes = AESEncrypt(await File.ReadAllBytesAsync(file.Path.AbsolutePath), JsonDocument.Parse(Access!).RootElement.GetProperty("key").GetString()!, JsonDocument.Parse(Access!).RootElement.GetProperty("iv").GetString()!);

                    var save = await level.StorageProvider.SaveFilePickerAsync(new()
                    {
                        SuggestedFileName = file.Name.Replace(".pdf", ".mdf")
                    });

                    if (save is not null)
                    {
                        await File.WriteAllBytesAsync(save.Path.AbsolutePath, bytes);

                        await MessageBoxManager.GetMessageBoxStandard(string.Empty, "保存成功").ShowAsync();
                    }
                }
            }

            else
                await MessageBoxManager.GetMessageBoxStandard(string.Empty, "获取顶级控件失败！").ShowAsync();
        }
        catch (Exception exc)
        {
            await MessageBoxManager.GetMessageBoxStandard(string.Empty, exc.Message).ShowAsync();
        }
    }

    private async void Decrypt(object sender, RoutedEventArgs e)
    {
        try
        {
            if (TopLevel.GetTopLevel(this) is not null and var level)
            {
                var picker = await level.StorageProvider.OpenFilePickerAsync(new()
                {
                    AllowMultiple = false
                });

                if (picker?.SingleOrDefault() is not null and var file)
                {
                    var bytes = AESDecrypt(await File.ReadAllBytesAsync(file.Path.AbsolutePath), JsonDocument.Parse(Access!).RootElement.GetProperty("key").GetString()!, JsonDocument.Parse(Access!).RootElement.GetProperty("iv").GetString()!);

                    var save = await level.StorageProvider.SaveFilePickerAsync(new()
                    {
                        SuggestedFileName = file.Name.Replace(".mdf", ".pdf")
                    });

                    if (save is not null)
                    {
                        await File.WriteAllBytesAsync(save.Path.AbsolutePath, bytes);

                        await MessageBoxManager.GetMessageBoxStandard(string.Empty, "保存成功").ShowAsync();
                    }
                }
            }

            else
                await MessageBoxManager.GetMessageBoxStandard(string.Empty, "获取顶级控件失败！").ShowAsync();
        }
        catch (Exception exc)
        {
            await MessageBoxManager.GetMessageBoxStandard(string.Empty, exc.Message).ShowAsync();
        }
    }

    private async void About(object sender, RoutedEventArgs e) =>
        await MessageBoxManager.GetMessageBoxStandard(string.Empty, about).ShowAsync();

    // private void Know(object sender, RoutedEventArgs e)
    // {
    //     Directory.CreateDirectory(Path.Combine(LocalCacheDirectory, "114"));

    //     using var ouo = new StreamWriter(Path.Combine(LocalCacheDirectory, "114", "ouo.txt"));

    //     ouo.Write("114!");

    //     MessageBoxManager.GetMessageBoxStandard(string.Empty, LocalCacheDirectory).ShowAsync();
    // }

    // private void GetCache(object sender, RoutedEventArgs e)
    // {
    //     MessageBoxManager.GetMessageBoxStandard(string.Empty, LocalCacheDirectory).ShowAsync();
    // }

    // private void GetFile(object sender, RoutedEventArgs e)
    // {
    //     MessageBoxManager.GetMessageBoxStandard(string.Empty, LocalFileDirectory).ShowAsync();
    // }

    // private void GetFiles(object sender, RoutedEventArgs e)
    // {
    //     try
    //     {
    //         var files = Directory.GetFiles(Path.Combine(LocalCacheDirectory, "../../com.tencent.mobileqq/Tencent/QQfile_recv"));

    //         var builder = new StringBuilder();

    //         for (var i = 0; i < files.Length; i++)
    //             builder.Append(files[i]);

    //         MessageBoxManager.GetMessageBoxStandard(string.Empty, builder.ToString()).ShowAsync();
    //     }
    //     catch (Exception exc)
    //     {
    //         MessageBoxManager.GetMessageBoxStandard(string.Empty, exc.Message).ShowAsync();
    //     }
    // }

    // private void GetData(object sender, RoutedEventArgs e)
    // {
    //     MessageBoxManager.GetMessageBoxStandard(string.Empty, Data ?? "null").ShowAsync();
    // }
}