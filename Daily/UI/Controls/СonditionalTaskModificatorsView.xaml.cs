using Daily.Tasks;

namespace Daily.Controls
{
    public partial class СonditionalTaskModificatorsView : ContentView
    {
        public static readonly BindableProperty FrequencyCardColorProperty =
            BindableProperty.Create(nameof(FrequencyCardColor), typeof(Color), typeof(СonditionalTaskModificatorsView));
        public static readonly BindableProperty CompletionTimeCardColorProperty =
            BindableProperty.Create(nameof(CompletionTimeCardColor), typeof(Color), typeof(СonditionalTaskModificatorsView));
        public static readonly BindableProperty NoteCardColorProperty =
            BindableProperty.Create(nameof(NoteCardColor), typeof(Color), typeof(СonditionalTaskModificatorsView));

        public static readonly BindableProperty FrequencyProperty =
            BindableProperty.Create(nameof(Frequency), typeof(string), typeof(СonditionalTaskModificatorsView));
        public static readonly BindableProperty CompletionTimeProperty =
            BindableProperty.Create(nameof(CompletionTime), typeof(int), typeof(СonditionalTaskModificatorsView));
        public static readonly BindableProperty NoteProperty =
            BindableProperty.Create(nameof(Note), typeof(string), typeof(СonditionalTaskModificatorsView));

        public Color FrequencyCardColor
        {
            get { return (Color)GetValue(FrequencyCardColorProperty); }
            set { SetValue(FrequencyCardColorProperty, value); }
        }

        public Color CompletionTimeCardColor
        {
            get { return (Color)GetValue(CompletionTimeCardColorProperty); }
            set { SetValue(CompletionTimeCardColorProperty, value); }
        }

        public Color NoteCardColor
        {
            get { return (Color)GetValue(NoteCardColorProperty); }
            set { SetValue(NoteCardColorProperty, value); }
        }

        public string Frequency
        {
            get { return (string)GetValue(FrequencyProperty); }
            set { SetValue(FrequencyProperty, value); }
        }

        public int CompletionTime
        {
            get { return (int)GetValue(CompletionTimeProperty); }
            set { SetValue(CompletionTimeProperty, value); }
        }

        public string Note
        {
            get { return (string)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        public СonditionalTaskModificatorsView()
        {
            InitializeComponent();
        }
    }
}