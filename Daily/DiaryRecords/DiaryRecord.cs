using Daily.Events;

namespace Daily.Diary
{
    public class DiaryRecord : NotifyPropertyChanged
    {
        private string _text;
        private DateTime _creationDateTime;

        public DateTime CreationDateTime
        {
            get => _creationDateTime;
            set
            {
                if (value == _creationDateTime) return;

                _creationDateTime = value;
                OnPropertyChanged(nameof(CreationDateTime));
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

        public DiaryRecord(string text, DateTime creationDateTime)
        {
            _text = text;
            _creationDateTime = creationDateTime;
        }
    }
}