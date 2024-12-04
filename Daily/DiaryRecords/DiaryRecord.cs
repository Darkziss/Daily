using Daily.Events;

namespace Daily.Diary
{
    public class DiaryRecord : NotifyPropertyChanged
    {
        private string _text;
        private readonly DateTime _creationDateTime;

        public DateTime CreationDateTime => _creationDateTime;

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

        public DiaryRecord(string text, DateTime creationDateTime)
        {
            _text = text;
            _creationDateTime = creationDateTime;
        }
    }
}