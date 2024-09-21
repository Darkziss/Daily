using Daily.ViewModels;

namespace Daily
{
    public partial class TaskPage : ContentPage
    {
        private static TaskPageViewModel _viewModel = new TaskPageViewModel();

        public TaskPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;

            _viewModel.PreparePage();
        }
    }
}