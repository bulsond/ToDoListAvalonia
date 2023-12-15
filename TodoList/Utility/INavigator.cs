using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.ViewModel;
using TodoList.Views;

namespace TodoList.Utility;

public interface INavigator
{
    IAppMainWindow AppMainWindow { get; set; }
    ServiceProvider ServiceProvider { get; set; }

    void RegisterRoute(string nameOfView, Type typeOfView);
    Task GoToAsync(string nameOfView);
    void GoToWaiting();
}

internal class Navigator : INavigator
{
    private readonly Dictionary<string, Type> _routes = new();

    public IAppMainWindow AppMainWindow { get; set; } = null!;
    public ServiceProvider ServiceProvider { get; set; } = null!;

    public void RegisterRoute(string nameOfView, Type typeOfView)
    {
        if (string.IsNullOrEmpty(nameOfView))
        {
            throw new ArgumentException($"'{nameof(nameOfView)}' не может быть null или empty.", nameof(nameOfView));
        }

        if (typeOfView is null)
        {
            throw new ArgumentNullException(nameof(typeOfView));
        }

        object? service = ServiceProvider.GetService(typeOfView);
        if (service is null)
        {
            throw new Exception($"Данный тип: {nameof(typeOfView)}, не зарегистрирован в DI контейнере.");
        }

        _routes.Add(nameOfView, typeOfView);
    }

    // TODO: сделать перегруженный вариант с передачей объектного параметра во въюмодель
    public async Task GoToAsync(string nameOfView)
    {
        if (string.IsNullOrEmpty(nameOfView))
        {
            throw new ArgumentException($"'{nameof(nameOfView)}' не может быть null или empty.", nameof(nameOfView));
        }

        if (_routes.ContainsKey(nameOfView) == false)
        {
            throw new ArgumentException($"'{nameof(nameOfView)}' не найден такой маршрут.", nameof(nameOfView));
        }

        Type viewType = _routes[nameOfView];
        object? service = ServiceProvider.GetService(viewType);
        if (service is null)
        {
            throw new Exception($"Данный тип: {nameof(viewType)} не зарегистрирован в ServiceProvider.");
        }

        UserControl view = (UserControl)service;
        ViewModelBase? viewModel = (view.DataContext as ViewModelBase);
        if (viewModel is null)
        {
            AppMainWindow.ShowView(view);
            return;
        }

        if (viewModel is IViewModelLoadable)
        {
            await (viewModel as IViewModelLoadable)!.LoadAsync();
        }

        AppMainWindow.ShowView(view);
    }

    public void GoToWaiting()
    {
        AppMainWindow.ShowView(null!);
    }
}
