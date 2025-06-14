using Daily.Events;

namespace Daily.Thoughts
{
    public class Thought : NotifyPropertyChanged
    {
        private string _name;
        private string _text;

        public string Name
        {
            get => _name;
            set
            {
                if (value.Equals(_name)) return;

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (value.Equals(_text)) return;

                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public Thought()
        {
            _name = string.Empty;
            _text = string.Empty;
        }

        public Thought(string name, string text)
        {
            _name = name;
            _text = text;
        }
    }
}