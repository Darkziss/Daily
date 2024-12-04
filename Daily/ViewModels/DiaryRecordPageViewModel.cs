using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Diary;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class DiaryRecordPageViewModel : ObservableObject
    {
        [ObservableProperty] private DiaryRecord? _selectedDiaryRecord = null;

        [ObservableProperty] private bool _canInteractWithDiaryRecord = true;
        [ObservableProperty] private bool _canDeleteThought = false;

        private readonly DiaryRecordStorage _diaryRecordStorage;

        public ObservableCollection<DiaryRecord> DiaryRecords => _diaryRecordStorage.DiaryRecords;

        public Command AddDiaryRecordCommand { get; }
            
        public DiaryRecordPageViewModel(DiaryRecordStorage diaryRecordStorage)
        {
            _diaryRecordStorage = diaryRecordStorage;

            AddDiaryRecordCommand = new Command(
            execute: async () =>
            {
                Debug.WriteLine(nameof(AddDiaryRecordCommand));

                CanInteractWithDiaryRecord = false;

                await PageNavigator.GoToDiaryRecordEditPageAsync();

                CanInteractWithDiaryRecord = true;
            });
        }
    }
}