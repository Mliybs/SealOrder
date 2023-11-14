using System.Text.Json.Nodes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace SealOrder.Static;

public static class Static
{
    // public static SocketIOClient.Transport.Http.IHttpClient Client => socket.HttpClient;

    // public static readonly SocketIOClient.SocketIO socket = new("wss://next.mliybs.top", new()
    // {
    //     Path = "/api/ws/",

    //     Auth = new { username = GetUserOption()["Username"]?.ToString() ?? "Mliybs", token = GetUserOption()["Token"]?.ToString() ?? "L3mBrxZipgB2kPOuj8sjm" },

    //     Transport = TransportProtocol.WebSocket
    // });

    public static JsonNode GetUserOption()
    {
        var path = Path.Combine(DataDirectory, "option.json");

        try
        {
            if (File.Exists(path))
                return JsonNode.Parse(File.ReadAllText(path) ?? "{}")!;

            else
                return JsonNode.Parse("{}")!;
        }
        catch (JsonException)
        {
            var option = JsonNode.Parse("{}")!;

            UpdateUserOption(option);

            return option;
        }
    }

    public static async void UpdateUserOption(JsonNode node) =>
        await File.WriteAllTextAsync(Path.Combine(DataDirectory, "option.json"), node?.ToString());

    public static void PlatformNotSupport(string? msg = null) => MessageBoxManager.GetMessageBoxStandard(string.Empty, msg ?? "此平台不支持该功能！");

    public static MsBox.Avalonia.Base.IMsBox<string> GetMessageBoxCustom(MsBox.Avalonia.Dto.MessageBoxCustomParams @params, MsBox.Avalonia.Controls.MsBoxCustomView view, MsBox.Avalonia.Enums.Icon icon = MsBox.Avalonia.Enums.Icon.None, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterScreen)
    {
        @params.Icon = icon;

        @params.WindowStartupLocation = windowStartupLocation;

        // @params.ButtonDefinitions = Array.Empty<MsBox.Avalonia.Models.ButtonDefinition>();

        var msBoxCustomViewModel = new MsBox.Avalonia.ViewModels.MsBoxCustomViewModel(@params);

        view.DataContext = msBoxCustomViewModel;

        return new NeoMsBox<MsBox.Avalonia.Controls.MsBoxCustomView,
        MsBox.Avalonia.ViewModels.MsBoxCustomViewModel,
        string>(view, msBoxCustomViewModel);
    }

    public static Task<byte[]> AESEncrypt(byte[] text, string key, string iv) => Task.Run(() =>
    {
        var cipher = CipherUtilities.GetCipher("AES/OFB/NoPadding");

        cipher.Init(true, new ParametersWithIV(new DesParameters(Encoding.UTF8.GetBytes(key)), Encoding.UTF8.GetBytes(iv)));

        return cipher.DoFinal(text);
    });

    public static async Task<byte[]> AESEncrypt(string text, string key, string iv) =>
        await AESEncrypt(Encoding.UTF8.GetBytes(text), key, iv);

    public static Task<byte[]> AESDecrypt(byte[] text, string key, string iv) => Task.Run(() =>
    {
        var cipher = CipherUtilities.GetCipher("AES/OFB/NoPadding");

        cipher.Init(true, new ParametersWithIV(new DesParameters(Encoding.UTF8.GetBytes(key)), Encoding.UTF8.GetBytes(iv)));

        var rv = new byte[cipher.GetOutputSize(text.Length)];

        var tam = cipher.ProcessBytes(text, 0, text.Length, rv, 0);

        cipher.DoFinal(rv, tam);

        return rv;
    });

    public static async Task<byte[]> AESDecrypt(string text, string key, string iv) =>
        await AESDecrypt(Encoding.UTF8.GetBytes(text), key, iv);

    private static string? dataDirectory = null;

    private static string? cacheDirectory = null;

    private static string? fileDirectory = null;

    public static string DataDirectory
    {
        get => dataDirectory ?? AppDomain.CurrentDomain.BaseDirectory;

        set => dataDirectory = value;
    }

    public static string LocalCacheDirectory
    {
        get => cacheDirectory ?? AppDomain.CurrentDomain.BaseDirectory;

        set => cacheDirectory = value;
    }

    public static string LocalFileDirectory
    {
        get => fileDirectory ?? AppDomain.CurrentDomain.BaseDirectory;

        set => fileDirectory = value;
    }

    public static (string Path, Func<byte[]> Bytes)? LoadedFile { get; set; }

    public static string? Access => File.Exists(Path.Combine(DataDirectory, "access.json")) ? File.ReadAllText(Path.Combine(DataDirectory, "access.json")) : null;

    public static Action<string>? Share { get; set; }

    public static Action<string>? Open { get; set; }

    public static Action? ToNotify { get; set; }

    public const string about = """
        MIT License

        Copyright (c) 2023 Mliybs

        Permission is hereby granted, free of charge, to any person obtaining a copy
        of this software and associated documentation files (the "Software"), to deal
        in the Software without restriction, including without limitation the rights
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the Software is
        furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all
        copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
        SOFTWARE.
        """;

    public static string ToMime(string name) => name switch
    {
        "txt" => "text/plain",
        "htm" or "html" => "text/html",
        "pdf" => "application/pdf",
        "jpg" or "jpeg" => "image/jpeg",
        "png" => "image/png",
        "gif" => "image/gif",
        "mp3" => "audio/mpeg",
        "mp4" => "video/mp4",
        "zip" => "application/zip",
        "rar" => "application/rar",
        "apk" => "application/vnd.android.package-archive",
        _ => "application/octet-stream"
    };
}