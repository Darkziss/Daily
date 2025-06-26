using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Thoughts
{
    public class ThoughtStorage
    {
        private readonly DataProvider _dataProvider;
        
        public ObservableCollection<Thought>? Thoughts { get; private set; }

        private const string thoughtIsNotOnListException = "Thought is not on list";

        public ThoughtStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<ObservableCollection<Thought>> LoadThoughts()
        {
            IEnumerable<Thought>? thoughts = await _dataProvider.LoadThoughtsAsync();

            Thoughts = thoughts == null ? new() : new(thoughts);

            return Thoughts;
        }

        public async Task<Thought?> TryCreateThoughtAsync(string name, string text)
        {
            if (!ValidateThoughtValues(name, text)) return null;

            name = name.Trim();
            text = text.Trim();
            Thought thought = new Thought(name, text);
            Thoughts.Insert(0, thought);

            await _dataProvider.SaveThoughtAsync(thought);

            return thought;
        }

        public async Task<bool> TryEditThoughtAsync(Thought thought, string name, string text)
        {
            bool contains = Thoughts.Contains(thought);

            if (!contains) throw new Exception(thoughtIsNotOnListException);

            bool isValid = ValidateThoughtValues(name, text);

            if (!isValid) return false;

            thought.Name = name.Trim();
            thought.Text = text.Trim();

            await _dataProvider.UpdateSavedThoughtAsync(thought);

            return true;
        }

        public async Task DeleteThoughtAsync(Thought thought)
        {
            if (thought == null) return;

            int index = Thoughts.IndexOf(thought);

            if (index == -1) throw new Exception(thoughtIsNotOnListException);

            Thoughts.RemoveAt(index);

            await _dataProvider.DeleteSavedThoughtAsync(thought);
        }

        private bool ValidateThoughtValues(string name, string text)
        {
            bool isNameValid = !string.IsNullOrWhiteSpace(name);
            bool isTextValid = !string.IsNullOrWhiteSpace(text);

            return isNameValid && isTextValid;
        }
    }
}