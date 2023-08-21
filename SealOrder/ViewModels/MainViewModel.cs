namespace SealOrder.ViewModels;

public class MainViewModel : ViewModelBase
{
    private string greeting = "原来，你也玩原神？";

    public string Greeting
    {
        get => greeting;

        set => this.RaiseAndSetIfChanged(ref greeting, value);
    }

    public void Click()
    {
        // MessageBoxManager.GetMessageBoxStandard("114", "514!").ShowAsync();
        new Window().Show();
    }
}
