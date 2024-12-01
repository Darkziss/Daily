using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class ThoughtPage : ContentPage
    {
        private readonly ThoughtPageViewModel _viewModel;
        
        public ThoughtPage(ThoughtPageViewModel viewModel)
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