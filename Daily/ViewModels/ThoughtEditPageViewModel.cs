using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class ThoughtEditPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isNameEntryReadOnly = true;

        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _text = string.Empty;

        public bool IsNameValid { get; set; }
        public bool IsTextValid { get; set; }

        [ObservableProperty] private bool _canSave = false;

        public Command<bool> ChangeNameReadOnlyCommand { get; }

        public ThoughtEditPageViewModel()
        {
            ChangeNameReadOnlyCommand = new Command<bool>((isReadOnly) => IsNameEntryReadOnly = isReadOnly);

            PropertyChanged += (_, args) =>
            {
                bool isThoughtProperty = args.PropertyName == nameof(Name) || args.PropertyName == nameof(Text);

                if (isThoughtProperty) CanSave = IsNameValid && IsTextValid;
            };
        }

        public void ResetView()
        {
            IsNameEntryReadOnly = true;

            Name = string.Empty;
            Text = string.Empty;

            CanSave = false;
        }
    }
}