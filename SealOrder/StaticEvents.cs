namespace SealOrder.Static;

public static partial class Static
{
    public static Action<string>? Share { get; set; }
    
    public static Action<string>? Open { get; set; }

    public static Action? ToNotify { get; set; }

    /// <summary>
    /// 调用后进行后退动作
    /// </summary>
    public static Action? BackPress { get; set; }

    /// <summary>
    /// 每当触发后退键时调用的委托
    /// </summary>
    public static Action? OnBackPress { get; set; }
}