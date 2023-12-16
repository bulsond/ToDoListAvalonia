using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TodoList.DataModel;
using TodoList.Service;
using TodoList.Utility;
using TodoList.View;

namespace TodoList.ViewModel;

public class ToDoListViewModel : ViewModelBase, IViewModelLoadable
{
    private readonly ToDoListService _service;
    private readonly INavigator _navigator;

    public ToDoListViewModel(ToDoListService service, INavigator navigator)
    {
        _service = service ?? throw new System.ArgumentNullException(nameof(service));
        _navigator = navigator ?? throw new System.ArgumentNullException(nameof(navigator));
    }

    public ObservableCollection<ToDoItem> ListItems { get; } = new();

    public async Task LoadAsync()
    {
        var items = await _service.GetItemsAsync();
        foreach (var item in items)
        {
            ListItems.Add(item);
        }
    }

    public async Task EditAsync(object arg)
    {
        if (arg is null)
        {
            return;
        }

        await _navigator.GoToAsync(nameof(AddItemView), arg);
    }

    public async Task DeleteAsync(object arg)
    {
        if (arg is null)
        {
            return;
        }
        var id = int.Parse((string)arg);
        // TODO: переключение на экран ожидания и обратно
        var answer = await _service.RemoveItemAsync(id);
        if (answer)
        {
            var item = ListItems.First(i => i.Id == id);
            ListItems.Remove(item);
        }
    }

    public async Task GoToAddItemAsync()
    {
        await _navigator.GoToAsync(nameof(AddItemView));
    }
}
