using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TodoList.DataModel;
using TodoList.Service;
using TodoList.Utility;
using TodoList.View;

namespace TodoList.ViewModel;

public partial class ToDoListViewModel : ViewModelBase, IViewModelLoadable
{
    private readonly ToDoListService _service;
    private readonly INavigator _navigator;

    public ToDoListViewModel(ToDoListService service, INavigator navigator)
    {
        _service = service ?? throw new System.ArgumentNullException(nameof(service));
        _navigator = navigator ?? throw new System.ArgumentNullException(nameof(navigator));
    }

    public ObservableCollection<ToDoItem> ListItems { get; } = new();

    [ObservableProperty]
    private ToDoItem selectedItem = null!;

    public async Task LoadAsync()
    {
        var items = await _service.GetItemsAsync();
        foreach (var item in items)
        {
            ListItems.Add(item);
        }
    }

    public async Task DeleteAsync()
    {
        
        if (SelectedItem is null)
        {
            return;
        }
        //bool result = await _service.RemoveItem(SelectedItem.Id);
        //if (result)
        //{
        //    ListItems.Remove(SelectedItem);
        //}
        await Task.Delay(10);
        ListItems.Remove(SelectedItem);

    }

    public async Task GoToAddItemAsync()
    {
        await _navigator.GoToAsync(nameof(AddItemView));
    }
}
