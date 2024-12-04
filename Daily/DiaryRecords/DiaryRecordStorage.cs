using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Diary
{
    public class DiaryRecordStorage
    {
        private readonly DataProvider _dataProvider;

        public ObservableCollection<DiaryRecord> DiaryRecords { get; }

        private const string diaryRecordIsNotOnListException = "Diary record is not on list";

        public DiaryRecordStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            if (_dataProvider.DiaryRecords == null) DiaryRecords = new ObservableCollection<DiaryRecord>();
            else DiaryRecords = new ObservableCollection<DiaryRecord>(_dataProvider.DiaryRecords);
        }

        public async Task<bool> TryAddDiaryRecordAsync(string text, DateTime creationTimeStamp)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            DiaryRecord diaryRecord = new DiaryRecord(text, creationTimeStamp);
            DiaryRecords.Insert(0, diaryRecord);

            await _dataProvider.SaveDiaryRecordsAsync(DiaryRecords);

            return true;
        }
    }
}