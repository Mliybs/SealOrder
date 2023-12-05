using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using SealOrder;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
    private static async Task Main(/* string[] args */) => await BuildAvaloniaApp()
            .UseReactiveUI()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = "avares://SealOrder.Browser/Assets/Fonts#Alibaba PuHuiTi 3.0",

                FontFallbacks = new FontFallback[]
                {
                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-35-Thin.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-45-Light.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-55-Regular.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-65-Medium.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-75-SemiBold.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-85-Bold.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-95-ExtraBold.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-105-Heavy.otf#Alibaba PuHuiTi 3.0"
                    },

                    new()
                    {
                        FontFamily = "avares://SealOrder.Browser/Assets/Fonts/AlibabaPuHuiTi-3-115-Black.otf#Alibaba PuHuiTi 3.0"
                    }
                }
            })
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}