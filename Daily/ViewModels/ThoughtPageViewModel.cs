using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Thoughts;
using Daily.Navigation;

namespace Daily.ViewModels
{
    public partial class ThoughtPageViewModel : ObservableObject
    {
        private readonly ThoughtStorage _thoughtStorage;

        public ObservableCollection<Thought> Thoughts => _thoughtStorage.Thoughts;

        public Command AddThoughtCommand { get; }
        
        public ThoughtPageViewModel(ThoughtStorage thoughtStorage)
        {
            _thoughtStorage = thoughtStorage;

            AddThoughtCommand = new Command(
            execute: async () =>
            {
                await PageNavigator.GoToThoughtEditPageAsync();
            });
        }
    }
}