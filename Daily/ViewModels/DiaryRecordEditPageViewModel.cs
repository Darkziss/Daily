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

            string creationDateTime = record.CreationDateTime.ToString(creationDateTimeFormat);
            HeaderText = $"Запись {creationDateTime}";
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
            bool success = record != null;

            if (!success)
            {
                await DiaryRecordToastHandler.ShowDiaryRecordErrorToastAsync();
                return;
            }

            if (record != _currentDiaryRecord) _currentDiaryRecord = record;

            await DiaryRecordToastHandler.ShowDiaryRecordCreatedToastAsync();
        }

        private async Task EditDiaryRecordAsync()
        {
            bool isSameText = _currentDiaryRecord!.Text == Text;

            if (isSameText) return;
            
            bool success = await _diaryRecordStorage.TryEditDiaryRecordAsync(_currentDiaryRecord!, Text);

            if (success) await DiaryRecordToastHandler.ShowDiaryRecordEditedToastAsync();
            else await DiaryRecordToastHandler.ShowDiaryRecordErrorToastAsync();
        }
    }
}