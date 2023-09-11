namespace SealOrder.ViewModels;

public class MenuBoxViewModel : ViewModelBase
{
    public MenuBoxViewModel()
    {
        Items = new List<string>();
    }

    public MenuBoxViewModel(List<string> list)
    {
        Items = list;
    }

    public MenuBoxViewModel(IEnumerable<string> collection)
    {
        Items = collection;
    }

    public MenuBoxViewModel(string hint, List<string> list)
    {
        Hint = hint;

        Items = list;
    }

    public MenuBoxViewModel(string hint, IEnumerable<string> collection)
    {
        Hint = hint;

        Items = collection;
    }

    public string? Hint { get; }

    public IEnumerable<string> Items { get; }
}