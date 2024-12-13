using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Thoughts;
using Daily.Navigation;
using Daily.Toasts;
using Daily.Popups;

namespace Daily.ViewModels
{
    public partial class ThoughtEditPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isEditMode = false;
        [ObservableProperty] private bool _canSave = false;

        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _text = string.Empty;

        [ObservableProperty] private bool _isNameEntryReadOnly = true;

        private Thought? _currentThought = null;
        
        private readonly ThoughtStorage _thoughtStorage;

        public bool IsNameValid { get; set; }
        public bool IsTextValid { get; set; }

        public Command<bool> ChangeNameReadOnlyCommand { get; }

        public Command SaveThoughtCommand { get; }
        public Command ActivateEditMode { get; }

        private bool ShouldPreventExit => (Name.Length > 0 || Text.Length > 0) && IsEditMode;

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
                CanSave = false;
                
                if (_currentThought == null) await CreateThoughtAsync();
                else await EditThoughtAsync();

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
            _currentThought = thought;
            
            IsEditMode = false;
            CanSave = false;

            Name = thought.Name;
            Text = thought.Text;

            IsNameEntryReadOnly = true;
        }

        public void ResetView()
        {
            _currentThought = null;
            
            IsEditMode = true;
            CanSave = false;

            Name = string.Empty;
            Text = string.Empty;

            IsNameEntryReadOnly = true;
        }

        public async Task PreventExitAsync()
        {
            if (!ShouldPreventExit)
            {
                await PageNavigator.ReturnToPreviousPage();
                return;
            }

            bool shouldLeave = await PopupHandler.ShowRecordExitPopupAsync();

            if (shouldLeave) await PageNavigator.ReturnToPreviousPage();
        }

        private async Task CreateThoughtAsync()
        {
            bool success = await _thoughtStorage.TryCreateThoughtAsync(Name, Text);

            if (success) await ThoughtToastHandler.ShowThoughtCreatedToastAsync();
            else await ThoughtToastHandler.ShowThoughtErrorToastAsync();
        }

        private async Task EditThoughtAsync()
        {
            bool isEdited = await _thoughtStorage.TryEditThoughtAsync(_currentThought!, Name, Text);

            if (isEdited) await ThoughtToastHandler.ShowThoughtEditedToastAsync();
            else await ThoughtToastHandler.ShowThoughtErrorToastAsync();
        }
    }
}