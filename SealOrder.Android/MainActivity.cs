﻿using Android;
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

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SealOrder.Static.Static.DataDirectory = DataDir!.AbsolutePath;

        SealOrder.Static.Static.LocalCacheDirectory = ExternalCacheDir!.AbsolutePath;

        SealOrder.Static.Static.LocalFileDirectory = GetExternalFilesDir(null)!.AbsolutePath;

        SealOrder.Static.Static.Share = dir =>
        {
            var uri = FileProvider.GetUriForFile(this, "com.Mlinetles.SealOrder", new Java.IO.File(dir));

            var intent = new Intent(Intent.ActionSend);

            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            intent.SetType(SealOrder.Static.Static.ToMime(dir.Replace(".mnd", string.Empty).Split('.')[^1]));

            intent.PutExtra(Intent.ExtraStream, uri);

            StartActivity(Intent.CreateChooser(intent, null as string));
        };

        SealOrder.Static.Static.Open = dir =>
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
            SealOrder.Static.Static.LoadedFile = (Intent.Data.Path!, () =>
            {
                var stream = new FileInputStream(ContentResolver!.OpenFileDescriptor(Intent.Data, "r")!.FileDescriptor);

                var bytes = new byte[stream.Available()];

                stream.Read(bytes);

                stream.Close();

                return bytes;
            });
        }

        SealOrder.Static.Static.Notify = () =>
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

                if (service.GetNotificationChannel("GenshinImpact").Importance == NotificationImportance.Default)
                {
                    var intent = new Intent(Settings.ActionChannelNotificationSettings);

                    intent.PutExtra(Settings.ExtraAppPackage, PackageName);

                    intent.PutExtra(Settings.ExtraChannelId, "GenshinImpact");

                    StartActivity(intent);
                }
            }
            catch (System.Exception e)
            {
                SealOrder.Static.MessageBoxManager.GetMessageBoxStandard(string.Empty, e.Message).ShowAsync();
            }
        };

        if (CheckSelfPermission(Manifest.Permission.PostNotifications) == Permission.Denied)
            RequestPermissions(new string[] { Manifest.Permission.PostNotifications }, 1);

        // SealOrder.Static.Static.socket.AddExpectedException(typeof(Java.Net.SocketException));
        
        // SealOrder.Static.Static.socket.AddExpectedException(typeof(Java.Net.SocketTimeoutException));
    }
}
