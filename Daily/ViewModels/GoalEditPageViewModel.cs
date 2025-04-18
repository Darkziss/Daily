using Daily.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

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

                CanSave = true;
            });
        }
    }
}