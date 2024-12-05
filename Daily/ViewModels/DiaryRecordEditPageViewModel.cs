using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Diary;
using Daily.Navigation;
using Daily.Toasts;

namespace Daily.ViewModels
{
    public partial class DiaryRecordEditPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isEditMode = false;
        [ObservableProperty] private bool _canSave = false;

        [ObservableProperty] private string _headerText = defaultHeaderText;
        [ObservableProperty] private string _text = string.Empty;

        private DiaryRecord? _currentDiaryRecord = null;

        private readonly DiaryRecordStorage _diaryRecordStorage;

        public Command SaveDiaryRecordCommand { get; }
        public Command ActivateEditMode { get; }

        private const string defaultHeaderText = "Новая запись";

        private const string creationDateTimeFormat = "d";

        public DiaryRecordEditPageViewModel(DiaryRecordStorage diaryRecordStorage)
        {
            _diaryRecordStorage = diaryRecordStorage;

            SaveDiaryRecordCommand = new Command(
            execute: async () =>
            {
                CanSave = false;

                if (_currentDiaryRecord == null) await CreateDiaryRecordAsync();

                await PageNavigator.ReturnToPreviousPage();
            });

            ActivateEditMode = new Command(() => IsEditMode = true);
        }

        private async Task CreateDiaryRecordAsync()
        {
            bool success = await _diaryRecordStorage.TryAddDiaryRecordAsync(Text, DateTime.Now);

            if (success) await DiaryRecordToastHandler.ShowDiaryRecordCreatedToastAsync();
            else await DiaryRecordToastHandler.ShowDiaryRecordErrorToastAsync();
        }

        public void PrepareViewForView(DiaryRecord record)
        {
            _currentDiaryRecord = record;

            IsEditMode = false;
            CanSave = true;

            string creationDateTime = record.CreationDateTime.ToString(creationDateTimeFormat);
            HeaderText = $"Запись {creationDateTime}";
            Text = record.Text;
        }

        public void ResetView()
        {
            _currentDiaryRecord = null;

            IsEditMode = true;
            CanSave = false;

            HeaderText = defaultHeaderText;
            Text = string.Empty;
        }
    }
}