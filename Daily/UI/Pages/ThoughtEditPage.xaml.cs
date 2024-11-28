using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class ThoughtEditPage : ContentPage
    {
        private readonly ThoughtEditPageViewModel _viewModel;

        public ThoughtEditPage(ThoughtEditPageViewModel viewModel)
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