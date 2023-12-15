using Avalonia.Controls;
using TodoList.ViewModel;

namespace TodoList.View
{
    public partial class AddItemView : UserControl
    {
        private readonly AddItemViewModel _viewModel = null!;

        public AddItemView()
        {
            InitializeComponent();
        }

        public AddItemView(AddItemViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = viewModel;
        }
    }
}
