using System.ComponentModel;

namespace Daily.Thoughts
{
    public class Thought : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public Thought(string name, string text)
        {
            _name = name;
            _text = text;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}