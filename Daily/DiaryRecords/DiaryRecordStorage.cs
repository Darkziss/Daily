using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Diary
{
    public class DiaryRecordStorage
    {
        private readonly IUnitOfWork _unitOfWork;

        public ObservableCollection<DiaryRecord> DiaryRecords { get; }

        private const string diaryRecordIsNotOnListException = "Diary record is not on list";

        public DiaryRecordStorage(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            IEnumerable<DiaryRecord> diaryRecords = unitOfWork.DiaryRecordRepository.GetAll();

            DiaryRecords = new(diaryRecords);
        }

        public DiaryRecord? TryAddDiaryRecord(string text, DateTime creationDateTime)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            text = text.Trim();
            DiaryRecord record = new DiaryRecord(text, creationDateTime);
            DiaryRecords.Insert(0, record);

            _unitOfWork.DiaryRecordRepository.Insert(record);
            
            return record;
        }

        public bool TryEditDiaryRecord(DiaryRecord record, string text)
        {
            bool contains = DiaryRecords.Contains(record);

            if (!contains) throw new Exception(diaryRecordIsNotOnListException);

            if (string.IsNullOrWhiteSpace(text)) return false;

            record.Text = text.Trim();

            _unitOfWork.DiaryRecordRepository.Update(record);

            return true;
        }

        public void DeleteDiaryRecord(DiaryRecord record)
        {
            if (record == null) return;

            int index = DiaryRecords.IndexOf(record);

            if (index == -1) throw new Exception(diaryRecordIsNotOnListException);

            DiaryRecords.RemoveAt(index);

            _unitOfWork.DiaryRecordRepository.Delete(record);
        }
    }
}