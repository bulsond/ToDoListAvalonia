using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Service;
using TodoList.Utility;
using TodoList.View;
using TodoList.ViewModel;
using TodoList.Views;

namespace TodoList;

public partial class App : Application
{
    private const double _MainWindowWidth = 400;
    private const double _MainWindowHeight = 550;

    public ServiceProvider ServiceProvider { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // создаем DI контейнер
        ServiceCollection services = new();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    // конфигурирование сервисов
    private void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<INavigator, Navigator>();
        services.AddTransient<MainView>();

        services.AddTransient<ToDoListView>();
        services.AddTransient<AddItemView>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<ToDoListViewModel>();
        services.AddTransient<AddItemViewModel>();

        services.AddSingleton<ToDoListService>();
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // получение ссылки на гл.окно через контейнер
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            // устанавливаем параметры гл. окна
            mainWindow!.Width = _MainWindowWidth;
            mainWindow!.Height = _MainWindowHeight;
            mainWindow!.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            // установка гл. окна приложения
            desktop.MainWindow = mainWindow;

            // получение ссылки на навигатор
            var navigator = ServiceProvider.GetService<INavigator>();
            // настройка навигатора: передача ссылки на гл.окно, на контейнер
            navigator!.AppMainWindow = mainWindow!;
            navigator!.ServiceProvider = ServiceProvider;

            // настройка маршрутов навигатора
            ConfigureRoutes(navigator);

            // переход к стартовой въюхе
            await navigator!.GoToAsync(nameof(ToDoListView));
        }

        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var mainView = ServiceProvider.GetService<MainView>();
            singleViewPlatform.MainView = mainView;
        }

        base.OnFrameworkInitializationCompleted();
    }

    // конфигурирование маршрутов
    private void ConfigureRoutes(INavigator navigator)
    {
        navigator.RegisterRoute(nameof(ToDoListView), typeof(ToDoListView));
        navigator.RegisterRoute(nameof(AddItemView), typeof(AddItemView));
    }
}
