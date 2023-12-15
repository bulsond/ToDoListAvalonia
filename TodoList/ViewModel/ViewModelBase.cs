using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace TodoList.ViewModel;

public class ViewModelBase : ObservableObject
{
    
}

public interface IViewModelLoadable
{
    // загрузка данных во въюшке
    public virtual Task LoadAsync() => Task.CompletedTask;
}
