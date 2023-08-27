namespace SealOrder.Static;

public static class Static
{
    private static string? directory = null;

    public static string LocalDirectory
    {
        get => directory ?? AppDomain.CurrentDomain.BaseDirectory;

        set => directory = value;
    }
}