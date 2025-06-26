using Daily.Diary;
using Daily.Thoughts;

namespace Daily.Data
{
    public interface IRepository
    {
        public Task<IReadOnlyList<DiaryRecord>> GetDiaryRecordsAsync();

        public Task<IEnumerable<Thought>> GetThoughtsAsync();
    }
}