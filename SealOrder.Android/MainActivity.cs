using Android;
using Android.App;
using Android.OS;
using Android.Net;
using Android.Provider;
using Android.Content;
using Android.Content.PM;
using AndroidX.Core.Content;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Java.IO;
using SealOrder.Static;

using static SealOrder.Static.Static;

namespace SealOrder.Android;

[Activity(
    Label = "SealOrder",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode),
IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault },
    DataMimeType = "*/*"
)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    public override void OnBackPressed()
    {
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        DataDirectory = DataDir!.AbsolutePath;

        LocalCacheDirectory = ExternalCacheDir!.AbsolutePath;

        LocalFileDirectory = GetExternalFilesDir(null)!.AbsolutePath;

        Share = dir =>
        {
            var uri = FileProvider.GetUriForFile(this, "com.Mlinetles.SealOrder", new Java.IO.File(dir));

            var intent = new Intent(Intent.ActionSend);

            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            intent.SetType(ToMime(dir.Replace(".mnd", string.Empty).Split('.')[^1]));

            intent.PutExtra(Intent.ExtraStream, uri);

            StartActivity(Intent.CreateChooser(intent, null as string));
        };

        Open = dir =>
        {
            var uri = FileProvider.GetUriForFile(this, "com.Mlinetles.SealOrder", new Java.IO.File(dir));

            var intent = new Intent(Intent.ActionView);

            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            intent.AddCategory(Intent.CategoryDefault);

            intent.SetDataAndType(uri, "application/pdf");

            StartActivity(intent);
        };

        if (Intent?.Data is not null)
        {
            LoadedFile = (Intent.Data.Path!, () =>
            {
                var stream = new FileInputStream(ContentResolver!.OpenFileDescriptor(Intent.Data, "r")!.FileDescriptor);

                var bytes = new byte[stream.Available()];

                stream.Read(bytes);

                stream.Close();

                return bytes;
            });
        }

        ToNotify = () =>
        {
            try
            {
                var service = GetSystemService(NotificationService) as NotificationManager;

                service.CreateNotificationChannel(new NotificationChannel("GenshinImpact", "原神专用", NotificationImportance.High));

                service.Notify(114514, new Notification.Builder(this, "GenshinImpact")
                    .SetContentTitle("我要玩原神！")
                    .SetStyle(new Notification.BigTextStyle().BigText($$"""
                    {{service.GetNotificationChannel("GenshinImpact").Importance.ToString()}}
                    """))
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .Build());

                if (service.GetNotificationChannel("GenshinImpact").Importance == NotificationImportance.High)
                {
                    var intent = new Intent(Settings.ActionAppNotificationSettings);

                    intent.PutExtra(Settings.ExtraAppPackage, PackageName);

                    StartActivity(intent);
                }
            }
            catch (System.Exception e)
            {
                MessageBoxManager.GetMessageBoxStandard(string.Empty, e.Message).ShowAsync();
            }
        };

        if (CheckSelfPermission(Manifest.Permission.PostNotifications) == Permission.Denied)
            RequestPermissions(new string[] { Manifest.Permission.PostNotifications }, 1);

        // SealOrder.Static.Static.socket.AddExpectedException(typeof(Java.Net.SocketException));
        
        // SealOrder.Static.Static.socket.AddExpectedException(typeof(Java.Net.SocketTimeoutException));
    }
}
