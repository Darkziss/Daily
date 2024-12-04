using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Diary;
using Daily.Navigation;

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

        public bool IsTextValid { get; set; }

        public Command SaveDiaryRecordCommand { get; }
        public Command ActivateEditMode { get; }

        private const string defaultHeaderText = "Новая запись";

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
        }

        private async Task CreateDiaryRecordAsync()
        {
            bool success = await _diaryRecordStorage.TryAddDiaryRecordAsync(Text, DateTime.Now);
        }

        public void ResetView()
        {
            throw new NotImplementedException();
        }
    }
}