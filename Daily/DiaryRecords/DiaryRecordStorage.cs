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

        public async Task<DiaryRecord?> TryAddDiaryRecordAsync(string text, DateTime creationDateTime)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            DiaryRecord record = new DiaryRecord(text, creationDateTime);
            DiaryRecords.Insert(0, record);

            await _dataProvider.SaveDiaryRecordsAsync(DiaryRecords);

            return record;
        }

        public async Task<bool> TryEditDiaryRecordAsync(DiaryRecord record, string text)
        {
            bool contains = DiaryRecords.Contains(record);

            if (!contains) throw new Exception(diaryRecordIsNotOnListException);

            if (string.IsNullOrWhiteSpace(text)) return false;

            record.Text = text;

            await _dataProvider.SaveDiaryRecordsAsync(DiaryRecords);

            return true;
        }

        public async Task DeleteDiaryRecordAsync(DiaryRecord record)
        {
            if (record == null) return;

            int index = DiaryRecords.IndexOf(record);

            if (index == -1) throw new Exception(diaryRecordIsNotOnListException);

            DiaryRecords.RemoveAt(index);

            await _dataProvider.SaveDiaryRecordsAsync(DiaryRecords);
        }
    }
}