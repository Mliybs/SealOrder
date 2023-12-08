namespace SealOrder.Views;

public partial class FocusTextBox : TextBox
{
    protected override Type StyleKeyOverride => typeof(TextBox);

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        // base.OnLostFocus(e);
    }

    public void LoseFocus()
    {
        base.OnLostFocus(new());
    }
}