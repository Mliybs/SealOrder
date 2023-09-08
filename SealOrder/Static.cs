using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace SealOrder.Static;

public static class Static
{
    public static void PlatformNotSupport() => MessageBoxManager.GetMessageBoxStandard(string.Empty, "此平台不支持该功能！");

    public static MsBox.Avalonia.Base.IMsBox<string> GetMessageBoxCustom(MsBox.Avalonia.Dto.MessageBoxCustomParams @params, MsBox.Avalonia.Controls.MsBoxCustomView view, MsBox.Avalonia.Enums.Icon icon = MsBox.Avalonia.Enums.Icon.None, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterScreen)
    {
        @params.Icon = icon;

        @params.WindowStartupLocation = windowStartupLocation;

        var msBoxCustomViewModel = new MsBox.Avalonia.ViewModels.MsBoxCustomViewModel(@params);

        view.DataContext = msBoxCustomViewModel;

        return new MsBox<MsBox.Avalonia.Controls.MsBoxCustomView,
        MsBox.Avalonia.ViewModels.MsBoxCustomViewModel,
        string>(view, msBoxCustomViewModel);
    }

    public static byte[] AESEncrypt(byte[] text, string key, string iv)
    {
        var cipher = CipherUtilities.GetCipher("AES/OFB/NoPadding");

        cipher.Init(true, new ParametersWithIV(new DesParameters(Encoding.UTF8.GetBytes(key)), Encoding.UTF8.GetBytes(iv)));

        return cipher.DoFinal(text);
    }

    public static byte[] AESEncrypt(string text, string key, string iv) =>
        AESEncrypt(Encoding.UTF8.GetBytes(text), key, iv);

    public static byte[] AESDecrypt(byte[] text, string key, string iv)
    {
        var cipher = CipherUtilities.GetCipher("AES/OFB/NoPadding");

        cipher.Init(true, new ParametersWithIV(new DesParameters(Encoding.UTF8.GetBytes(key)), Encoding.UTF8.GetBytes(iv)));

        var rv = new byte[cipher.GetOutputSize(text.Length)];

        var tam = cipher.ProcessBytes(text, 0, text.Length, rv, 0);

        cipher.DoFinal(rv, tam);

        return rv;
    }

    public static byte[] AESDecrypt(string text, string key, string iv) =>
        AESDecrypt(Encoding.UTF8.GetBytes(text), key, iv);

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

    public static string? Access { get => File.Exists(Path.Combine(DataDirectory, "access.json")) ? File.ReadAllText(Path.Combine(DataDirectory, "access.json")) : null; }

    public static Action<string>? Share { get; set; }

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
}