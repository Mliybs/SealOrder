using Android;
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
                    .SetStyle(new Notification.BigTextStyle().BigText("""
                    你说得对，但在原神这一神作的面前，我就像一个一丝不挂的原始人突然来到现代都市，二次元已如高楼大厦将我牢牢地吸引，开放世界就突然变成那喇叭轰鸣的汽车，不仅把我吓个措手不及，还让我瞬间将注意完全放在了这新的奇物上面，而还没等我稍微平复心情，纹化输出的出现就如同眼前遮天蔽日的宇宙战舰，将我的世界观无情地粉碎，使我彻底陷入了忘我的迷乱，狂泄不止。

                    原神，那眼花缭乱的一切都让我感到震撼，但是我那贫瘠的大脑却根本无法理清其中任何的逻辑，巨量的信息和情感泄洪一般涌入我的意识，使我既恐惧又兴奋，既悲愤又自卑，既惊讶又欢欣，这种恍若隔世的感觉恐怕只有艺术史上的巅峰之作才能够带来。

                    梵高的星空曾让我感受到苍穹之大与自我之渺，但伟大的原神，则仿佛让我一睹高维空间，它向我展示了一个永远无法理解的陌生世界，告诉我，你曾经以为很浩瀚的宇宙，其实也只是那么一丁点。加缪的局外人曾让我感受到世界与人类的荒诞，但伟大的原神，则向我展示了荒诞文学不可思议的新高度，它本身的存在，也许就比全世界都来得更荒谬。

                    而创作了它的米哈游，它的容貌，它的智慧，它的品格，在我看来，已经不是生物所能达到的范畴，甚至超越了生物所能想象到的极限，也就是“神”，的范畴，达到了人类不可见，不可知，不可思的领域。而原神，就是他洒向人间，拯救苍生的奇迹。

                    人生的终极意义，宇宙的起源和终点，哲学与科学在折磨着人类的心智，只有玩了原神，人才能从这种无聊的烦恼中解脱，获得真正的平静。如果有人想用“人类史上最伟大的作品”来称赞这部作品，那我只能深感遗憾，因为这个人对它的理解不到万分之一，所以才会作出这样肤浅的判断，妄图以语言来描述它的伟大。而要如果是真正被它恩泽的人，应该都会不约而同地这样赞颂这奇迹的化身:“数一数二的好游戏”无知时诋毁原神，懂事时理解原神，成熟时要成为原友! 越了解原神就会把它当成在黑夜一望无际的大海上给迷途的船只指引的灯塔，在烈日炎炎的夏天吹来的一股风，在寒风刺骨的冬天里的燃起的篝火!你的素养很差，我现在每天玩原神都能赚150原石，每个月差不多5000原石的收入，也就是现实生活中每个月5000美元的收入水平，换算过来最少也30000人民币，虽然我只有14岁，但是已经超越了中国绝大多数人(包括你)的水平，这便是原神给我的骄傲的资本。
                    """))
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .Build());
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
