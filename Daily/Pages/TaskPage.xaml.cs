using Daily.ViewModels;

namespace Daily
{
    public partial class TaskPage : ContentPage
    {
        public TaskPage(TaskPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;

            viewModel.PreparePage();
        }
    }
}