using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Thoughts;
using Daily.Navigation;
using Daily.Toasts;
using Daily.Popups;
using AsyncTimer = System.Timers.Timer;
using Sharpnado.TaskLoaderView;

namespace Daily.ViewModels
{
    public partial class ThoughtPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isLoaded = false;
        
        [ObservableProperty] private Thought? _selectedThought = null;

        [ObservableProperty] private bool _canInteractWithThought = true;
        [ObservableProperty] private bool _canDeleteThought = false;

        private bool _isThoughtOpened = false;
        
        private readonly ThoughtStorage _thoughtStorage;

        public TaskLoaderNotifier<ObservableCollection<Thought>> Loader { get; }

        public Command<Thought> ThoughtInteractCommand { get; }

        public Command AddThoughtCommand { get; }

        public Command SwitchCanDeleteCommand { get; }

        private const double dummyDelay = 800d;
        
        public ThoughtPageViewModel(ThoughtStorage thoughtStorage)
        {
            _thoughtStorage = thoughtStorage;

            Loader = new(true);

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

                    _isThoughtOpened = true;

                    await PageNavigator.GoToThoughtEditPageAsync(parameters);
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

            if (Loader.IsNotStarted)
                Loader.Load(_ => _thoughtStorage.LoadThoughts());

            if (!_isThoughtOpened)
            {
                ShowDummy();
            }
            else
            {
                IsLoaded = true;
                CanInteractWithThought = true;
            }

            _isThoughtOpened = false;
        }

        private void ShowDummy()
        {
            IsLoaded = false;
            CanInteractWithThought = false;

            AsyncTimer timer = new AsyncTimer(dummyDelay);

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();

                IsLoaded = true;
                CanInteractWithThought = true;
            };

            timer.Start();
        }
    }
}