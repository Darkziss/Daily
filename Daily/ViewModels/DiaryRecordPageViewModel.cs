using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Diary;
using Daily.Navigation;
using Daily.Popups;
using Daily.Toasts;
using AsyncTimer = System.Timers.Timer;

namespace Daily.ViewModels
{
    public partial class DiaryRecordPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isLoaded = false;

        [ObservableProperty] private DiaryRecord? _selectedDiaryRecord = null;

        [ObservableProperty] private bool _canInteractWithDiaryRecord = true;
        [ObservableProperty] private bool _canDeleteDiaryRecord = false;

        private readonly DiaryRecordStorage _diaryRecordStorage;

        public ObservableCollection<DiaryRecord> DiaryRecords => _diaryRecordStorage.DiaryRecords;

        public Command<DiaryRecord> DiaryRecordInteractCommand { get; }

        public Command AddDiaryRecordCommand { get; }

        public Command SwitchCanDeleteCommand { get; }

        private bool ShouldLoad => DiaryRecords.Count > 0;

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
                        await DiaryRecordToastHandler.ShowDiaryRecordDeletedToastAsync();
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

            if (ShouldLoad) ShowDummy();
            else
            {
                IsLoaded = true;
                CanInteractWithDiaryRecord = true;
            }
        }

        private void ShowDummy()
        {
            IsLoaded = false;
            CanInteractWithDiaryRecord = false;
            
            const double delay = 800d;
            AsyncTimer timer = new AsyncTimer(delay);

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();

                IsLoaded = true;
                CanInteractWithDiaryRecord = true;
            };

            timer.Start();
        }
    }
}