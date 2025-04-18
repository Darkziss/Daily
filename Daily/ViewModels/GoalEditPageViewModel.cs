using Daily.Tasks;
using Daily.Navigation;
using Daily.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Daily.ViewModels
{
    public partial class GoalEditPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _canSave = true;

        [ObservableProperty] private string _goal;
        [ObservableProperty] private DateTime _deadline;

        private readonly GoalStorage _goalStorage;

        public Command SaveCommand { get; }

        public GoalEditPageViewModel(GoalStorage goalStorage)
        {
            _goalStorage = goalStorage;

            _goal = goalStorage.Goal;
            _deadline = DateTime.Now;

            SaveCommand = new Command(async () =>
            {
                CanSave = false;

                await _goalStorage.SetGoalAsync(Goal);

                WeakReferenceMessenger.Default.Send<GoalChangedMessage>();

                await PageNavigator.ReturnToPreviousPageAsync();

                CanSave = true;
            });
        }

        public void PrepareView()
        {
            Goal = _goalStorage.Goal;
            Deadline = DateTime.Now;
        }
    }
}