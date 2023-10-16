using System.Text.Json.Nodes;
using Avalonia.Controls;
using DynamicData;

namespace SealOrder.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void Load(object sender, RoutedEventArgs e)
    {
        if (Directory.Exists(Path.Combine(LocalCacheDirectory, "public/temp")))
            Directory.Delete(Path.Combine(LocalCacheDirectory, "public/temp"), true);

        Directory.CreateDirectory(Path.Combine(LocalCacheDirectory, "public/temp"));

        if (Access is null)
            await MessageBoxManager.GetMessageBoxStandard(string.Empty, "未保存通行等级！").ShowAsync();

        else
            if (LoadedFile is not null)
                try
                {
                    if (TopLevel.GetTopLevel(this) is not null and var level)
                    {
                        var name = LoadedFile.Value.Path[(LoadedFile.Value.Path.LastIndexOf('/') + 1)..];

                        var access = JsonDocument.Parse(Access).RootElement;

                        Func<byte[], string, string, Task<byte[]>>? operate = null;

                        switch (await MessageBoxManager.GetMessageBoxCustom(new()
                        {
                            ContentMessage = "请选择加密或解密该文件",

                            WindowStartupLocation = WindowStartupLocation.CenterScreen,

                            ButtonDefinitions = new MsBox.Avalonia.Models.ButtonDefinition[]
                            {
                                new()
                                {
                                    Name = "加密"
                                },
                                
                                new()
                                {
                                    Name = "解密"
                                }
                            }
                        }).ShowAsync())
                        {
                            case "加密":

                                operate = AESEncrypt;

                                break;

                            case "解密":

                                operate = AESDecrypt;

                                break;
                        }

                        var index = await GetMessageBoxCustom(new(), new()
                        {
                            Content = new MenuBox("请选择通行等级", access.EnumerateObject().Select(x => x.Name))
                        }).ShowAsync();

                        if (index is null)
                            return;

                        var bytes = await operate!.Invoke(LoadedFile.Value.Bytes(), access.GetProperty(index).GetProperty("key").GetString()!, access.GetProperty(index).GetProperty("iv").GetString()!);

                        switch (await MessageBoxManager.GetMessageBoxCustom(new()
                        {
                            ContentMessage = "请选择保存或分享该文件",

                            WindowStartupLocation = WindowStartupLocation.CenterScreen,

                            ButtonDefinitions = name.EndsWith(".mdf") && Open is not null
                            ? new MsBox.Avalonia.Models.ButtonDefinition[]
                            {
                                new()
                                {
                                    Name = "保存"
                                },

                                new()
                                {
                                    Name = "打开"
                                },
                                
                                new()
                                {
                                    Name = "分享"
                                }
                            }
                            : new MsBox.Avalonia.Models.ButtonDefinition[]
                            {
                                new()
                                {
                                    Name = "保存"
                                },
                                
                                new()
                                {
                                    Name = "分享"
                                }
                            }
                        }).ShowAsync())
                        {
                            case "保存":

                                var save = await level.StorageProvider.SaveFilePickerAsync(new()
                                {
                                    SuggestedFileName = name.Replace(".mnd", string.Empty)
                                });

                                if (save is not null)
                                {
                                    using var write = await save.OpenWriteAsync();
                                    
                                    write.Write(bytes);

                                    await MessageBoxManager.GetMessageBoxStandard(string.Empty, "保存成功").ShowAsync();
                                }

                                break;

                            case "分享":

                                File.WriteAllBytes(Path.Combine(LocalCacheDirectory, "public/temp", name.Replace(".mnd", string.Empty)), bytes);

                                if (Share is not null)
                                    Share.Invoke(Path.Combine(LocalCacheDirectory, "public/temp", name.Replace(".mnd", string.Empty)));

                                else
                                    PlatformNotSupport();

                                break;

                            case "打开":

                                File.WriteAllBytes(Path.Combine(LocalCacheDirectory, "public/temp", name.Replace(".mnd", string.Empty)), bytes);

                                Open!.Invoke(Path.Combine(LocalCacheDirectory, "public/temp", name.Replace(".mnd", string.Empty)));

                                break;
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

    private async void Loading(object sender, RoutedEventArgs e)
    {
        var text = await GetMessageBoxCustom(new(), new()
        {
            Content = new InputBox("请输入通行等级"),
        }).ShowAsync();

        if (text is not null)
        {
            var access = JsonNode.Parse(Access ?? "{}")!;

            string? name = null;

            string? key = null;

            string? iv = null;

            foreach (var item in text.Split(' '))
            {
                switch (item.Split('：'))
                {
                    case ["等级", var value]:

                        name = value;

                        break;

                    case ["密钥", var value]:

                        key = value;

                        break;

                    case ["向量", var value]:

                        iv = value;

                        break;

                    default:

                        await MessageBoxManager.GetMessageBoxStandard(string.Empty, "通行等级不合法！").ShowAsync();

                        return;
                }
            }

            if (name is null || key is null || iv is null)
            {
                await MessageBoxManager.GetMessageBoxStandard(string.Empty, "通行等级不合法！").ShowAsync();

                return;
            }

            access[name] = new JsonObject();

            access[name]!["key"] = key;

            access[name]!["iv"] = iv;

            await File.WriteAllTextAsync(Path.Combine(DataDirectory, "access.json"), access.ToString());

            await MessageBoxManager.GetMessageBoxStandard(string.Empty, "保存成功！").ShowAsync();
        }
    }

    private async void Encrypt(object sender, RoutedEventArgs e)
    {
        if (Access is null)
            await MessageBoxManager.GetMessageBoxStandard(string.Empty, "未保存通行等级！").ShowAsync();

        else
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
                        using var stream = await file.OpenReadAsync();

                        var buffer = new byte[stream.Length];

                        stream.Read(buffer);

                        var access = JsonDocument.Parse(Access).RootElement;

                        var index = await GetMessageBoxCustom(new(), new()
                        {
                            Content = new MenuBox("请选择通行等级", access.EnumerateObject().Select(x => x.Name))
                        }).ShowAsync();

                        if (index is null)
                            return;

                        var bytes = await AESDecrypt(buffer, access.GetProperty(index).GetProperty("key").GetString()!, access.GetProperty(index).GetProperty("iv").GetString()!);

                        switch (await MessageBoxManager.GetMessageBoxCustom(new()
                        {
                            ContentMessage = "请选择保存或分享该文件",

                            WindowStartupLocation = WindowStartupLocation.CenterScreen,

                            ButtonDefinitions = new MsBox.Avalonia.Models.ButtonDefinition[]
                            {
                                new()
                                {
                                    Name = "保存"
                                },
                                
                                new()
                                {
                                    Name = "分享"
                                }
                            }
                        }).ShowAsync())
                        {
                            case "保存":

                                var save = await level.StorageProvider.SaveFilePickerAsync(new()
                                {
                                    SuggestedFileName = file.Name + "mnd"
                                });

                                if (save is not null)
                                {
                                    using var write = await save.OpenWriteAsync();
                                    
                                    write.Write(bytes);

                                    await MessageBoxManager.GetMessageBoxStandard(string.Empty, "保存成功").ShowAsync();
                                }

                                break;

                            case "分享":

                                File.WriteAllBytes(Path.Combine(LocalCacheDirectory, "public/temp", file.Name + "mnd"), bytes);

                                if (Share is not null)
                                    Share.Invoke(Path.Combine(LocalCacheDirectory, "public/temp", file.Name + "mnd"));

                                else
                                    PlatformNotSupport();

                                break;
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
        if (Access is null)
            await MessageBoxManager.GetMessageBoxStandard(string.Empty, "未保存通行等级！").ShowAsync();

        else
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
                        using var stream = await file.OpenReadAsync();

                        var buffer = new byte[stream.Length];

                        stream.Read(buffer);

                        var access = JsonDocument.Parse(Access).RootElement;

                        var index = await GetMessageBoxCustom(new(), new()
                        {
                            Content = new MenuBox("请选择通行等级", access.EnumerateObject().Select(x => x.Name))
                        }).ShowAsync();

                        if (index is null)
                            return;

                        var bytes = await AESDecrypt(buffer, access.GetProperty(index).GetProperty("key").GetString()!, access.GetProperty(index).GetProperty("iv").GetString()!);

                        switch (await MessageBoxManager.GetMessageBoxCustom(new()
                        {
                            ContentMessage = "请选择保存或分享该文件",

                            WindowStartupLocation = WindowStartupLocation.CenterScreen,

                            ButtonDefinitions = file.Name.EndsWith(".mdf") && Open is not null
                            ? new MsBox.Avalonia.Models.ButtonDefinition[]
                            {
                                new()
                                {
                                    Name = "保存"
                                },

                                new()
                                {
                                    Name = "打开"
                                },
                                
                                new()
                                {
                                    Name = "分享"
                                }
                            }
                            : new MsBox.Avalonia.Models.ButtonDefinition[]
                            {
                                new()
                                {
                                    Name = "保存"
                                },
                                
                                new()
                                {
                                    Name = "分享"
                                }
                            }
                        }).ShowAsync())
                        {
                            case "保存":

                                var save = await level.StorageProvider.SaveFilePickerAsync(new()
                                {
                                    SuggestedFileName = file.Name.Replace(".mnd", string.Empty)
                                });

                                if (save is not null)
                                {
                                    using var write = await save.OpenWriteAsync();
                                    
                                    write.Write(bytes);

                                    await MessageBoxManager.GetMessageBoxStandard(string.Empty, "保存成功").ShowAsync();
                                }

                                break;

                            case "分享":

                                File.WriteAllBytes(Path.Combine(LocalCacheDirectory, "public/temp", file.Name.Replace(".mnd", string.Empty)), bytes);

                                if (Share is not null)
                                    Share.Invoke(Path.Combine(LocalCacheDirectory, "public/temp", file.Name.Replace(".mnd", string.Empty)));

                                else
                                    PlatformNotSupport();

                                break;

                            case "打开":

                                File.WriteAllBytes(Path.Combine(LocalCacheDirectory, "public/temp", file.Name.Replace(".mnd", string.Empty)), bytes);

                                Open!.Invoke(Path.Combine(LocalCacheDirectory, "public/temp", file.Name.Replace(".mnd", string.Empty)));

                                break;
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

    private void ChangeView(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton button)
        {
            switch (button.Name)
            {
                case "Chat":

                    this.GetControl<DockPanel>("Root").Children[1] = new ChatView();

                    break;

                case "Settings":

                    this.GetControl<DockPanel>("Root").Children[1] = new ChatView();

                    break;
            }
        }
    }
}