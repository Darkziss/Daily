using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Diary;
using Daily.Navigation;

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
            
        public DiaryRecordPageViewModel(DiaryRecordStorage diaryRecordStorage)
        {
            _diaryRecordStorage = diaryRecordStorage;

            DiaryRecordInteractCommand = new Command<DiaryRecord>(
            execute: async (record) =>
            {
                if (SelectedDiaryRecord == null || !CanInteractWithDiaryRecord) return;

                CanInteractWithDiaryRecord = false;

                if (CanDeleteDiaryRecord) return;
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
        }

        public void ResetView()
        {
            CanDeleteDiaryRecord = false;
        }
    }
}