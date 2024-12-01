using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Thoughts;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class ThoughtPageViewModel : ObservableObject
    {
        [ObservableProperty] private Thought? _selectedThought = null;

        [ObservableProperty] private bool _canInteractWithThought = true;
        
        private readonly ThoughtStorage _thoughtStorage;

        public ObservableCollection<Thought> Thoughts => _thoughtStorage.Thoughts;

        public Command<Thought> ViewThoughtCommand { get; }

        public Command AddThoughtCommand { get; }
        
        public ThoughtPageViewModel(ThoughtStorage thoughtStorage)
        {
            _thoughtStorage = thoughtStorage;

            ViewThoughtCommand = new Command<Thought>(
            execute: async (thought) =>
            {
                if (SelectedThought == null || !CanInteractWithThought) return;

                CanInteractWithThought = false;

                var parameters = new ShellNavigationQueryParameters()
                {
                    [nameof(Thought)] = thought
                };

                await PageNavigator.GoToThoughtEditPageWithParametersAsync(parameters);

                SelectedThought = null;
            });
            
            AddThoughtCommand = new Command(
            execute: async () =>
            {
                await PageNavigator.GoToThoughtEditPageAsync();
            });
        }
    }
}