using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.DataModel;

namespace TodoList.Service;

public class ToDoListService
{
    private List<ToDoItem> _toDoItems;

    public ToDoListService()
    {
        _toDoItems = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Description = "Walk the dog" },
            new ToDoItem { Id = 2, Description = "Buy some milk" },
            new ToDoItem { Id = 3, Description = "Learn Avalonia", IsChecked = true },
        };
    }
    public async Task<IEnumerable<ToDoItem>> GetItemsAsync()
    {
        await Task.Delay(1000);
        return _toDoItems.ToArray();
    }

    public async Task AddItemAsync(ToDoItem item)
    {
        item.Id = _toDoItems.Max(i => i.Id) + 1;
        await Task.Delay(1000);
        _toDoItems.Add(item);
    }

    public async Task<bool> RemoveItem(int id)
    {
        await Task.Delay(1000);

        ToDoItem? item = _toDoItems.FirstOrDefault(i => i.Id == id);
        if (item is null)
        {
            return false;
        }
        _toDoItems.Remove(item);
        return true;
    }
}
