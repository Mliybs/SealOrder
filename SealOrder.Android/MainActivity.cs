using Android.App;
using Android.OS;
using Android.Net;
using Android.Content;
using Android.Content.PM;
using AndroidX.Core.Content;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Java.IO;

namespace SealOrder.Android;

[Activity(
    Label = "SealOrder",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode),
IntentFilter(
    new[] { "android.intent.action.VIEW" },
    Categories = new[] { "android.intent.category.DEFAULT" },
    DataMimeType = "application/octet-stream"
)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SealOrder.Static.Static.DataDirectory = DataDir!.AbsolutePath;

        SealOrder.Static.Static.LocalCacheDirectory = ExternalCacheDir!.AbsolutePath;

        SealOrder.Static.Static.LocalFileDirectory = GetExternalFilesDir(null)!.AbsolutePath;

        SealOrder.Static.Static.Share = dir =>
        {
            var uri = FileProvider.GetUriForFile(this, PackageName, new Java.IO.File(dir));

            var intent = new Intent("Intent.ACTION_SEND");

            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            intent.SetType("application/octet-stream");

            intent.PutExtra(Intent.ExtraStream, uri);

            StartActivity(intent);
        };

        if (Intent?.Data is not null)
        {
            SealOrder.Static.Static.LoadedFile = (Intent.Data.Path!, () =>
            {
                var stream = new FileInputStream(ContentResolver.OpenFileDescriptor(Intent.Data, "r")!.FileDescriptor);

                var bytes = new byte[stream.Available()];

                stream.Read(bytes);

                stream.Close();

                return bytes;
            });
        }
    }
}
