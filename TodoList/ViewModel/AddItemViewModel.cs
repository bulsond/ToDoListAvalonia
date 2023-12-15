using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using TodoList.DataModel;
using TodoList.Service;
using TodoList.Utility;
using TodoList.View;

namespace TodoList.ViewModel;

public partial class AddItemViewModel : ViewModelBase
{
    private readonly INavigator _navigator = null!;
    private readonly ToDoListService _service = null!;

    public AddItemViewModel() { }

    public AddItemViewModel(INavigator navigator, ToDoListService service)
    {
        _navigator = navigator;
        _service = service;
    }

    [ObservableProperty]
    private string description = string.Empty;

    public async Task CheckAndGoToItemsAsync(object arg)
    {
        _navigator.GoToWaiting();
        var item = new ToDoItem { Description = Description.Trim() };

        await _service.AddItemAsync(item);
        await _navigator.GoToAsync(nameof(ToDoListView));
    }

    public bool CanCheckAndGoToItemsAsync(object arg)
    {
        return string.IsNullOrWhiteSpace(Description.Trim()) == false;
    }

    public async Task GoBackToItemsViewAsync()
    {
        _navigator.GoToWaiting();
        await _navigator.GoToAsync(nameof(ToDoListView));
    }

}
