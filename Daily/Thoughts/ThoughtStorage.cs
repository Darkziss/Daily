using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Thoughts
{
    public class ThoughtStorage
    {
        private readonly DataProvider _dataProvider;
        
        public ObservableCollection<Thought> Thoughts { get; }

        private const string thoughtIsNotOnListException = "Thought is not on list";

        public ThoughtStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            Thoughts = new ObservableCollection<Thought>();
        }

        public async Task<bool> TryCreateThoughtAsync(string name, string text)
        {
            if (!ValidateThoughtValues(name, text)) return false;
            
            Thought thought = new Thought(0, name, text);
            Thoughts.Add(thought);

            await _dataProvider.SaveThoughtsAsync(Thoughts);

            return true;
        }

        public async Task EditThoughtAsync(Thought thought, string name, string text)
        {
            bool contains = Thoughts.Contains(thought);

            if (!contains) throw new Exception(thoughtIsNotOnListException);

            thought.Name = name;
            thought.Text = text;

            await _dataProvider.SaveThoughtsAsync(Thoughts);
        }

        private bool ValidateThoughtValues(string name, string text)
        {
            bool isNameValid = !string.IsNullOrWhiteSpace(name);
            bool isTextValid = !string.IsNullOrWhiteSpace(text);

            return isNameValid && isTextValid;
        }
    }
}