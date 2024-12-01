using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Thoughts;
using Daily.Navigation;
using Daily.Toasts;
using Daily.Popups;

namespace Daily.ViewModels
{
    public partial class ThoughtPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private Thought? _selectedThought = null;

        [ObservableProperty] private bool _canInteractWithThought = true;

        [ObservableProperty] private bool _canDeleteThought = false;
        
        private readonly ThoughtStorage _thoughtStorage;

        public ObservableCollection<Thought> Thoughts => _thoughtStorage.Thoughts;

        public Command<Thought> ThoughtInteractCommand { get; }

        public Command AddThoughtCommand { get; }

        public Command SwitchCanDeleteCommand { get; }
        
        public ThoughtPageViewModel(ThoughtStorage thoughtStorage)
        {
            _thoughtStorage = thoughtStorage;

            ThoughtInteractCommand = new Command<Thought>(
            execute: async (thought) =>
            {
                if (SelectedThought == null || !CanInteractWithThought) return;

                CanInteractWithThought = false;

                if (CanDeleteThought)
                {
                    bool shouldDelete = await PopupHandler.ShowThoughtDeletePopupAsync();

                    if (shouldDelete)
                    {
                        await _thoughtStorage.DeleteThoughtAsync(thought);
                        await ThoughtToastHandler.ShowThoughtDeletedToastAsync();
                    }
                }
                else
                {
                    var parameters = new ShellNavigationQueryParameters()
                    {
                        [nameof(Thought)] = thought
                    };

                    await PageNavigator.GoToThoughtEditPageWithParametersAsync(parameters);
                }

                SelectedThought = null;
                CanInteractWithThought = true;
            });
            
            AddThoughtCommand = new Command(
            execute: async () =>
            {
                CanInteractWithThought = false;
                
                await PageNavigator.GoToThoughtEditPageAsync();

                CanInteractWithThought = true;
            });

            SwitchCanDeleteCommand = new Command(() => CanDeleteThought = !CanDeleteThought);
        }

        public void ResetView()
        {
            CanDeleteThought = false;
        }
    }
}