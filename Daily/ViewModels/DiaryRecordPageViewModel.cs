using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Diary;
using Daily.Navigation;
using Daily.Popups;
using Daily.Toasts;

namespace Daily.ViewModels
{
    public partial class DiaryRecordPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private DiaryRecord? _selectedDiaryRecord = null;

        [ObservableProperty] private bool _canInteractWithDiaryRecord = true;
        [ObservableProperty] private bool _canDeleteDiaryRecord = false;

        private readonly DiaryRecordStorage _diaryRecordStorage;

        public ObservableCollection<DiaryRecord> DiaryRecords => _diaryRecordStorage.DiaryRecords;

        public Command<DiaryRecord> DiaryRecordInteractCommand { get; }

        public Command AddDiaryRecordCommand { get; }

        public Command SwitchCanDeleteCommand { get; }
            
        public DiaryRecordPageViewModel(DiaryRecordStorage diaryRecordStorage)
        {
            _diaryRecordStorage = diaryRecordStorage;

            DiaryRecordInteractCommand = new Command<DiaryRecord>(
            execute: async (record) =>
            {
                if (SelectedDiaryRecord == null || !CanInteractWithDiaryRecord) return;

                CanInteractWithDiaryRecord = false;

                if (CanDeleteDiaryRecord)
                {
                    bool shouldDelete = await PopupHandler.ShowDiaryRecordDeletePopupAsync();

                    if (shouldDelete)
                    {
                        await _diaryRecordStorage.DeleteDiaryRecordAsync(record);
                        await ThoughtToastHandler.ShowThoughtDeletedToastAsync();
                    }
                }
                else
                {
                    var parameters = new ShellNavigationQueryParameters()
                    {
                        [nameof(DiaryRecord)] = record
                    };

                    await PageNavigator.GoToDiaryRecordEditPageWithParametersAsync(parameters);
                }

                SelectedDiaryRecord = null;
                CanInteractWithDiaryRecord = true;
            });

            AddDiaryRecordCommand = new Command(
            execute: async () =>
            {
                CanInteractWithDiaryRecord = false;

                await PageNavigator.GoToDiaryRecordEditPageAsync();

                CanInteractWithDiaryRecord = true;
            });

            SwitchCanDeleteCommand = new Command(() => CanDeleteDiaryRecord = !CanDeleteDiaryRecord);
        }

        public void ResetView()
        {
            CanDeleteDiaryRecord = false;
        }
    }
}