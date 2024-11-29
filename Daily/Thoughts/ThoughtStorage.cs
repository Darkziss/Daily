using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Thoughts
{
    public class ThoughtStorage
    {
        private readonly DataProvider _dataProvider;
        
        public ObservableCollection<Thought> Thoughts { get; private set; }

        private const string thoughtIsNotOnListException = "Thought is not on list";

        public ThoughtStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            Thoughts = new ObservableCollection<Thought>()
            {
                new Thought(0, "Настоящий ты это когда все плохо", "Настоящий ты - это когда все плохо, когда сложно и не понятно, " +
                "а не когда все хорошо"),
                new Thought(1, "Сомнения", "Не забывайте сомневаться в своих сомнениях"),
                new Thought(2, "А", "А"),
                new Thought(3, "Б", "Б")
            };
        }

        public async Task<Thought> AddThoughtAsync()
        {
            Thought thought = new Thought(0, string.Empty, string.Empty);

            await _dataProvider.SaveThoughtsAsync(Thoughts);

            return thought;
        }

        public async Task EditThoughtAsync(Thought thought, string name, string text)
        {
            bool contains = Thoughts.Contains(thought);

            if (!contains) throw new Exception(thoughtIsNotOnListException);

            thought.Name = name;
            thought.Text = text;

            await _dataProvider.SaveThoughtsAsync(Thoughts);
        }
    }
}