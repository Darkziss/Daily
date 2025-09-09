using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class GoalEditPage : ContentPage
    {
        private readonly GoalEditPageViewModel _viewModel;

        public GoalEditPage(GoalEditPageViewModel viewModel)
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