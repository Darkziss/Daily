using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class TaskPage : ContentPage
    {
        private readonly TaskPageViewModel _viewModel;
        
        public TaskPage(TaskPageViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            _viewModel.ResetView();
        }
    }
}