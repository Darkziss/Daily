using Daily.Diary;
using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class DiaryRecordEditPage : ContentPage, IQueryAttributable
    {
        private DiaryRecordEditPageViewModel _viewModel;
        
        public DiaryRecordEditPage(DiaryRecordEditPageViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = viewModel;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            bool haveDiaryRecord = query.ContainsKey(nameof(DiaryRecord));

            if (haveDiaryRecord)
            {
                DiaryRecord record = (DiaryRecord)query[nameof(DiaryRecord)];

                _viewModel.PrepareViewForView(record);
            }
            else _viewModel.ResetView();
        }

        protected override bool OnBackButtonPressed()
        {
            Dispatcher.DispatchAsync(async () => await _viewModel.PreventExitAsync());

            return true;
        }
    }
}