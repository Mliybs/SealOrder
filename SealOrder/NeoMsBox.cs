using Avalonia.Diagnostics;
using DialogHostAvalonia;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Windows;
using MsBox.Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.ViewModels;

namespace SealOrder.Static;

public class NeoMsBox<V, VM, T> : IMsBox<T>  where V : UserControl, IFullApi<T>, ISetCloseAction where VM : ISetFullApi<T>, IInput
{
    private readonly V _view;
    private readonly VM _viewModel;

    public string InputValue { get { return _viewModel.InputValue; } }

    public NeoMsBox(V view, VM viewModel)
    {
        _view = view;

        _viewModel = viewModel;
    }

    public Task<T> ShowAsync()
    {
        if (Application.Current != null &&
            Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return ShowWindowAsync();

        if (Application.Current != null &&
            Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
            return ShowAsPopupAsync((lifetime.MainView as ContentControl)!);

        throw new NotSupportedException("ApplicationLifetime is not supported");
    }
    
    public Task<T> ShowWindowAsync()
    {
        _viewModel.SetFullApi(_view);

        var window = new MsBoxWindow
        {
            Content = _view,

            DataContext = _viewModel
        };

        window.Closed += _view.CloseWindow;

        var tcs = new TaskCompletionSource<T>();

        _view.SetCloseAction(() =>
        {
            tcs.TrySetResult(_view.GetButtonResult());
            window.Close();
        });

        window.Show();

        return tcs.Task;
    }

    public Task<T> ShowWindowDialogAsync(Window owner)
    {
        _viewModel.SetFullApi(_view);

        var window = new MsBoxWindow
        {
            Content = _view,

            DataContext = _viewModel
        };

        window.Closed += _view.CloseWindow;

        var tcs = new TaskCompletionSource<T>();

        _view.SetCloseAction(() =>
        {
            var result = _view.GetButtonResult();

            window.Close();

            tcs.TrySetResult(result);
        });

        window.ShowDialog(owner);

        return tcs.Task;
    }

    public Task<T> ShowAsPopupAsync(ContentControl owner)
    {
        DialogHostStyles? style = null;

        if (!owner.Styles.OfType<DialogHostStyles>().Any())
        {
            style = new DialogHostStyles();
            
            owner.Styles.Add(style);
        }


        var parentContent = owner.Content;

        var dh = new DialogHost
        {
            Identifier = "MsBoxIdentifier" + Guid.NewGuid()
        };

        _viewModel.SetFullApi(_view);

        owner.Content = null;

        dh.Content = parentContent;

        dh.CloseOnClickAway = false;

        owner.Content = dh;

        var tcs = new TaskCompletionSource<T>();

        _view.SetCloseAction(() =>
        {
            var result = _view.GetButtonResult();

            DialogHost.Close(dh.Identifier);

            owner.Content = null;

            dh.Content = null;
            
            owner.Content = parentContent;

            if (style != null)
                owner.Styles.Remove(style);

            tcs.TrySetResult(result);
        });

        DialogHost.Show(_view, dh.Identifier);

        return tcs.Task;
    }

    public Task<T> ShowAsPopupAsync(Window owner)
    {
        return ShowAsPopupAsync(owner as ContentControl);
    }
}

public static class MessageBoxManager
{
    public static IMsBox<string> GetMessageBoxCustom(MessageBoxCustomParams @params)
    {
        var msBoxCustomViewModel = new MsBoxCustomViewModel(@params);

        return new NeoMsBox<MsBoxCustomView, MsBoxCustomViewModel, string>(new MsBoxCustomView
        {
            DataContext = msBoxCustomViewModel
        }, msBoxCustomViewModel);
    }

    public static IMsBox<ButtonResult> GetMessageBoxStandard(MessageBoxStandardParams @params)
    {
        var msBoxStandardViewModel = new MsBoxStandardViewModel(@params);

        return new NeoMsBox<MsBoxStandardView, MsBoxStandardViewModel, ButtonResult>(new MsBoxStandardView
        {
            DataContext = msBoxStandardViewModel
        }, msBoxStandardViewModel);
    }

    public static IMsBox<ButtonResult> GetMessageBoxStandard(string title, string text, ButtonEnum @enum = ButtonEnum.Ok, Icon icon = Icon.None, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterScreen)
    {
        return GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = title,

            ContentMessage = text,

            ButtonDefinitions = @enum,

            Icon = icon,
            
            WindowStartupLocation = windowStartupLocation
        });
    }
}