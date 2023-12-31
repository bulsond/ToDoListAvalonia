﻿using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.ViewModel;
using TodoList.Views;

namespace TodoList.Utility;

/// <summary>
/// Навигатор перехода между страницами в приложении
/// </summary>
public interface INavigator
{
    /// <summary>
    /// ссылка на главное окно приложения
    /// </summary>
    IAppMainWindow AppMainWindow { get; set; }

    /// <summary>
    /// ссылка на DI контейнер
    /// </summary>
    ServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// регистрация маршрута в навигаторе
    /// </summary>
    /// <param name="nameOfView">наименование въюшки (используется как ключ для поиска)</param>
    /// <param name="typeOfView">тип въюшки (typeOf(...))</param>
    void RegisterRoute(string nameOfView, Type typeOfView);

    /// <summary>
    /// переход на нужную страницу приложения
    /// </summary>
    /// <param name="nameOfView">наименование въюшки (используется как ключ для поиска)</param>
    /// <param name="parameter">опциональный параметр для передачи во въюшку,
    /// целевая въюшка должна реализовывать IViewModelParameterized</param>
    /// <returns></returns>
    Task GoToAsync(string nameOfView, object? parameter = default);

    /// <summary>
    /// отображение страницы ожидания подгрузки данных
    /// </summary>
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

    public async Task GoToAsync(string nameOfView, object? parameter = default)
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

        var view = (UserControl)service;
        ViewModelBase? viewModel = (view.DataContext as ViewModelBase);
        if (viewModel is null)
        {
            // просто въюха не имеет своей въюмодели
            AppMainWindow.ShowView(view);
            return;
        }

        if (viewModel is IViewModelLoadable)
        {
            // надо подгрузить данные во въюмодель
            await (viewModel as IViewModelLoadable)!.LoadAsync();
        }

        if (viewModel is IViewModelParameterized)
        {
            // передаем аргумент во въюмодель
            await (viewModel as IViewModelParameterized)!.SetParameterAsync(parameter);
        }

        AppMainWindow.ShowView(view);
    }

    public void GoToWaiting()
    {
        AppMainWindow.ShowView(null!);
    }
}
