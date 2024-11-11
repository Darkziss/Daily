using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class TaskEditPage : ContentPage
    {
        private readonly TaskEditPageViewModel _viewModel;

        public TaskEditPage(TaskEditPageViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            _viewModel.PrepareView();
        }
    }
}