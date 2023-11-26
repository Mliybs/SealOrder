namespace SealOrder.Android;

[Service]
public class MainService : Service
{
    public override void OnCreate()
    {
        base.OnCreate();
    }

    public override IBinder? OnBind(Intent? intent)
    {
        throw new NotImplementedException();
    }
}