using Android.App;
using Android.OS;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;

namespace SealOrder.Android;

[Activity(
    Label = "SealOrder",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }
}

public static class Initial
{
    public static bool IsInitialized = Initialize();

    public static bool Initialize()
    {
        if (Environment.MediaMounted.Equals(Environment.ExternalStorageState))
            SealOrder.Static.Static.LocalDirectory = Environment.ExternalStorageDirectory.AbsolutePath;

        return true;
    }
}