using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class DiaryRecordPage : ContentPage
    {
        private readonly DiaryRecordPageViewModel _viewModel;

        public DiaryRecordPage(DiaryRecordPageViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}