using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using TodoList.DataModel;
using TodoList.Service;
using TodoList.Utility;
using TodoList.View;

namespace TodoList.ViewModel;

public partial class AddItemViewModel : ViewModelBase, IViewModelParameterized
{
    private readonly INavigator _navigator = null!;
    private readonly ToDoListService _service = null!;
    private bool _isEditItem = false;
    private int _itemId = 0;

    public AddItemViewModel() { }

    public AddItemViewModel(INavigator navigator, ToDoListService service)
    {
        _navigator = navigator;
        _service = service;
    }

    [ObservableProperty]
    private string description = string.Empty;

    public async Task SetParameterAsync(object value)
    {
        if (value is null)
        {
            return;
        }
        var id = int.Parse((string)value);
        ToDoItem? item = await _service.GetItemAsync(id);
        if (item is null)
        {
            return;
        }
        Description = item.Description;
        _itemId = id;
        _isEditItem = true;
    }

    public async Task CheckAndGoToItemsAsync(object arg)
    {
        _navigator.GoToWaiting();
        var item = new ToDoItem { Id = _itemId, Description = Description.Trim() };
                
        if (_isEditItem)
        {
            await _service.EditItemAsync(item);
        }
        else
        {
            await _service.AddItemAsync(item);
        }
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
