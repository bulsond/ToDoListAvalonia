using Avalonia.Controls;
using TodoList.ViewModel;

namespace TodoList.Views;

public interface IAppMainWindow
{
    void ShowView(UserControl view);
}

public partial class MainWindow : Window, IAppMainWindow
{
    private readonly MainViewModel _viewModel = null!;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        Output.Content = _viewModel.Greeting;
    }

    public void ShowView(UserControl view)
    {
        if (view is null)
        {
            Output.Content = _viewModel.Greeting;
            return;
        }

        Output.Content = view;
    }
}
