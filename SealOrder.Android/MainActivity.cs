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

        System.IO.Directory.CreateDirectory(System.IO.Path.Combine(ExternalCacheDir!.AbsolutePath, "public/temp"));

        SealOrder.Static.Static.Share = dir =>
        {
            try
            {
                var uri = FileProvider.GetUriForFile(this, PackageName, new Java.IO.File(dir));

                System.IO.File.WriteAllText(System.IO.Path.Combine(ExternalCacheDir!.AbsolutePath, "uri.log"), uri.Path);

                var intent = new Intent();

                intent.SetAction("Intent.ACTION_SEND");

                intent.SetDataAndType(uri, "application/octet-stream");

                intent.AddFlags(ActivityFlags.FlagActivityNewTask);

                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                StartActivity(Intent.CreateChooser(intent, "请选择分享至的软件"));
            }
            catch (System.Exception e)
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(ExternalCacheDir!.AbsolutePath, "error.log"), e.Message);
            }
        };

        if (Intent?.Data is not null)
        {
            SealOrder.Static.Static.LoadedFile = (Intent.Data.Path!, () =>
            {
                var stream = new FileInputStream(ContentResolver!.OpenFileDescriptor(Intent.Data, "r")!.FileDescriptor);

                var bytes = new byte[stream.Available()];

                stream.Read(bytes);

                stream.Close();

                return bytes;
            });
        }
    }
}
