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
        [ObservableProperty] private bool _canInteract = false;

        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _text = string.Empty;

        private Thought? _currentThought = null;
        
        private readonly ThoughtStorage _thoughtStorage;

        public bool IsNameValid { get; set; }
        public bool IsTextValid { get; set; }

        public Command InteractWithThoughtCommand { get; }

        private bool ShouldPreventExit => (Name.Length > 0 || Text.Length > 0) && IsEditMode;

        public ThoughtEditPageViewModel(ThoughtStorage thoughtStorage)
        {
            _thoughtStorage = thoughtStorage;

            InteractWithThoughtCommand = new Command(async () =>
            {
                if (!IsEditMode)
                {
                    IsEditMode = true;
                    return;
                }

                CanInteract = false;

                if (_currentThought == null) await CreateThoughtAsync();
                else await EditThoughtAsync();

                IsEditMode = false;
                CanInteract = true;
            });

            PropertyChanged += (_, args) =>
            {
                bool isThoughtProperty = args.PropertyName == nameof(Name) || args.PropertyName == nameof(Text);

                if (isThoughtProperty && IsEditMode)
                {
                    CanInteract = IsNameValid && IsTextValid;
                }
            };
        }

        public void PrepareViewForView(Thought thought)
        {
            _currentThought = thought;
            
            IsEditMode = false;
            CanInteract = true;

            Name = thought.Name;
            Text = thought.Text;
        }

        public void ResetView()
        {
            _currentThought = null;
            
            IsEditMode = true;
            CanInteract = false;

            Name = string.Empty;
            Text = string.Empty;
        }

        public async Task PreventExitAsync()
        {
            if (!ShouldPreventExit)
            {
                await PageNavigator.ReturnToPreviousPageAsync();
                return;
            }

            bool shouldLeave = await PopupHandler.ShowRecordExitPopupAsync();

            if (shouldLeave) await PageNavigator.ReturnToPreviousPageAsync();
        }

        private async Task CreateThoughtAsync()
        {
            Thought? thought = await _thoughtStorage.TryCreateThoughtAsync(Name, Text);
            
            if (thought == null)
            {
                await ThoughtToastHandler.ShowThoughtErrorToastAsync();
                return;
            }

            _currentThought = thought;
            Name = _currentThought.Name;
            Text = _currentThought.Text;

            await ThoughtToastHandler.ShowThoughtCreatedToastAsync();
        }

        private async Task EditThoughtAsync()
        {
            bool isSame = _currentThought!.Name == Name && _currentThought!.Text == Text;

            if (isSame) return;
            
            bool success = await _thoughtStorage.TryEditThoughtAsync(_currentThought!, Name, Text);

            if (success)
            {
                Name = _currentThought.Name;
                Text = _currentThought.Text;
                
                await ThoughtToastHandler.ShowThoughtEditedToastAsync();
            }
            else await ThoughtToastHandler.ShowThoughtErrorToastAsync();
        }
    }
}