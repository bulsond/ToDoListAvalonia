using Avalonia.Controls;
using TodoList.ViewModel;

namespace TodoList.View
{
    public partial class ToDoListView : UserControl
    {
        private readonly ToDoListViewModel _viewModel = null!;

        public ToDoListView()
        {
            InitializeComponent();
        }

        public ToDoListView(ToDoListViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = viewModel;
        }
    }
}
