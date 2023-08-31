namespace SealOrder.Static;

public static class Static
{
    private static string? cacheDirectory = null;

    private static string? fileDirectory = null;

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

    public static string? Data { get; set; }
}