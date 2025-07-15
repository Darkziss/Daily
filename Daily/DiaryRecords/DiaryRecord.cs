using Daily.Events;
using SQLite;

namespace Daily.Diary
{
    public class DiaryRecord : NotifyPropertyChanged
    {
        private string _text;
        private DateTime _creationDateTime;

        [PrimaryKey, AutoIncrement] public int Id { get; set; }

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

        public DiaryRecord()
        {
            _text = string.Empty;
            _creationDateTime = DateTime.MinValue;
        }

        public DiaryRecord(string text, DateTime creationDateTime)
        {
            _text = text;
            _creationDateTime = creationDateTime;
        }
    }
}