using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Diary;
using Daily.Navigation;
using Daily.Popups;
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

        private bool ShouldPreventExit => Text.Length > 0 && IsEditMode;

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
                else await EditDiaryRecordAsync();

                IsEditMode = false;
                CanSave = true;
            });

            ActivateEditMode = new Command(() => IsEditMode = true);
        }

        public async Task PreventExitAsync()
        {
            if (!ShouldPreventExit)
            {
                await PageNavigator.ReturnToPreviousPage();
                return;
            }

            bool shouldLeave = await PopupHandler.ShowRecordExitPopupAsync();

            if (shouldLeave) await PageNavigator.ReturnToPreviousPage();
        }

        public void PrepareViewForView(DiaryRecord record)
        {
            _currentDiaryRecord = record;

            IsEditMode = false;

            SetHeaderTextByDate(_currentDiaryRecord.CreationDateTime);
            Text = record.Text;
        }

        public void ResetView()
        {
            _currentDiaryRecord = null;

            IsEditMode = true;

            HeaderText = defaultHeaderText;
            Text = string.Empty;
        }

        private async Task CreateDiaryRecordAsync()
        {
            DiaryRecord? record = await _diaryRecordStorage.TryAddDiaryRecordAsync(Text, DateTime.Now);

            if (record == null)
            {
                await DiaryRecordToastHandler.ShowDiaryRecordErrorToastAsync();
                return;
            }

            _currentDiaryRecord = record;
            SetHeaderTextByDate(record.CreationDateTime);

            await DiaryRecordToastHandler.ShowDiaryRecordCreatedToastAsync();
        }

        private async Task EditDiaryRecordAsync()
        {
            bool isSame = _currentDiaryRecord!.Text == Text;

            if (isSame) return;
            
            bool success = await _diaryRecordStorage.TryEditDiaryRecordAsync(_currentDiaryRecord!, Text);

            if (success) await DiaryRecordToastHandler.ShowDiaryRecordEditedToastAsync();
            else await DiaryRecordToastHandler.ShowDiaryRecordErrorToastAsync();
        }

        private void SetHeaderTextByDate(DateTime creationDateTime)
        {
            string date = creationDateTime.ToString(creationDateTimeFormat);
            HeaderText = $"Запись {date}";
        }
    }
}