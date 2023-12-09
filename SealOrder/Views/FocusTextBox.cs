
namespace SealOrder.Views;

public partial class FocusTextBox : TextBox
{
    public FocusTextBox()
    {
        TextInputOptions.SetContentType(this, TextInputContentType.Social);
    }

    protected override Type StyleKeyOverride => typeof(TextBox);

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
    }

    public void LoseFocus()
    {
        base.OnLostFocus(new());
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
    }
}