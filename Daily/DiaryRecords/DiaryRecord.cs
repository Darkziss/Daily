using Daily.Events;

namespace Daily.Diary
{
    public class DiaryRecord : NotifyPropertyChanged
    {
        private string _text;
        private readonly DateTime _creationTimeStamp;

        public DateTime CreationTimeStamp => _creationTimeStamp;

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

        public DiaryRecord(string text, DateTime creationTimeStamp)
        {
            _text = text;
            _creationTimeStamp = creationTimeStamp;
        }
    }
}