using System.Collections.ObjectModel;
using Daily.Data;

namespace Daily.Thoughts
{
    public class ThoughtStorage
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public ObservableCollection<Thought> Thoughts { get; }

        private const string thoughtIsNotOnListException = "Thought is not on list";

        public ThoughtStorage(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            IEnumerable<Thought> thoughts = _unitOfWork.ThoughtRepository.GetAll();

            Thoughts = new(thoughts);
        }

        public Thought? TryCreateThoughtAsync(string name, string text)
        {
            if (!ValidateThoughtValues(name, text)) return null;

            name = name.Trim();
            text = text.Trim();
            Thought thought = new Thought(name, text);
            Thoughts.Insert(0, thought);

            _unitOfWork.ThoughtRepository.Insert(thought);

            return thought;
        }

        public bool TryEditThoughtAsync(Thought thought, string name, string text)
        {
            bool contains = Thoughts.Contains(thought);

            if (!contains) throw new Exception(thoughtIsNotOnListException);

            bool isValid = ValidateThoughtValues(name, text);

            if (!isValid) return false;

            thought.Name = name.Trim();
            thought.Text = text.Trim();

            _unitOfWork.ThoughtRepository.Update(thought);

            return true;
        }

        public void DeleteThoughtAsync(Thought thought)
        {
            if (thought == null) return;

            int index = Thoughts.IndexOf(thought);

            if (index == -1) throw new Exception(thoughtIsNotOnListException);

            Thoughts.RemoveAt(index);

            _unitOfWork.ThoughtRepository.Delete(thought);
        }

        private bool ValidateThoughtValues(string name, string text)
        {
            bool isNameValid = !string.IsNullOrWhiteSpace(name);
            bool isTextValid = !string.IsNullOrWhiteSpace(text);

            return isNameValid && isTextValid;
        }
    }
}