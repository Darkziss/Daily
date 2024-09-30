using CommunityToolkit.Mvvm.ComponentModel;

namespace Daily.ViewModels
{
    public partial class TaskEditPageViewModel : ObservableObject
    {
        [ObservableProperty] private int _repeatCountLabelText = 1;
        
        public Command ChangeRepeatCount { get; }

        public TaskEditPageViewModel()
        {
            ChangeRepeatCount = new Command(
            execute: (count) =>
            {
                RepeatCountLabelText = (int)count;
            });
        }
    }
}