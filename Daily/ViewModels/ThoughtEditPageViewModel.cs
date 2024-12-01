using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Thoughts;
using Daily.Navigation;
using Daily.Toasts;

namespace Daily.ViewModels
{
    public partial class ThoughtEditPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isEditMode = false;
        [ObservableProperty] private bool _canSave = false;

        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _text = string.Empty;

        [ObservableProperty] private bool _isNameEntryReadOnly = true;
        
        private readonly ThoughtStorage _thoughtStorage;

        public bool IsNameValid { get; set; }
        public bool IsTextValid { get; set; }

        public Command<bool> ChangeNameReadOnlyCommand { get; }

        public Command SaveThoughtCommand { get; }
        public Command ActivateEditMode { get; }

        public ThoughtEditPageViewModel(ThoughtStorage thoughtStorage)
        {
            _thoughtStorage = thoughtStorage;

            ChangeNameReadOnlyCommand = new Command<bool>((isReadOnly) =>
            {
                if (!IsEditMode) return;
                
                IsNameEntryReadOnly = isReadOnly;
            });

            SaveThoughtCommand = new Command(
            execute: async () =>
            {
                bool success = await _thoughtStorage.TryCreateThoughtAsync(Name, Text);

                if (success) await ThoughtToastHandler.ShowThoughtCreatedToastAsync();
                else await ThoughtToastHandler.ShowThoughtErrorToastAsync();

                await PageNavigator.ReturnToPreviousPage();
            });

            ActivateEditMode = new Command(() => IsEditMode = true);

            PropertyChanged += (_, args) =>
            {
                bool isThoughtProperty = args.PropertyName == nameof(Name) || args.PropertyName == nameof(Text);

                if (isThoughtProperty) CanSave = IsNameValid && IsTextValid;
            };
        }

        public void PrepareViewForView(Thought thought)
        {
            IsEditMode = false;

            Name = thought.Name;
            Text = thought.Text;

            CanSave = false;

            IsNameEntryReadOnly = true;
        }

        public void ResetView()
        {
            IsEditMode = true;

            Name = string.Empty;
            Text = string.Empty;

            CanSave = false;

            IsNameEntryReadOnly = true;
        }
    }
}