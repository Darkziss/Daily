using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Daily.Thoughts;

namespace Daily.ViewModels
{
    public partial class ThoughtPageViewModel : ObservableObject
    {
        private readonly ThoughtStorage _thoughtStorage;

        public ObservableCollection<Thought> Thoughts => _thoughtStorage.Thoughts;
        
        public ThoughtPageViewModel(ThoughtStorage thoughtStorage)
        {
            _thoughtStorage = thoughtStorage;
        }
    }
}