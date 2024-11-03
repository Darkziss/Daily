using Daily.ViewModels;

namespace Daily
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        
        public MainPage(MainPageViewModel viewModel)
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