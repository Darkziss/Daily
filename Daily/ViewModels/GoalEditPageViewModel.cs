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
        [ObservableProperty] private DateOnly _deadline;

        private readonly GoalStorage _goalStorage;

        public Command SaveCommand { get; }

        private bool NeedDeadline => _deadline > CurrentDate;

        private static DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.Now);

        public GoalEditPageViewModel(GoalStorage goalStorage)
        {
            _goalStorage = goalStorage;

            _goal = goalStorage.Goal;
            _deadline = _goalStorage.Deadline ?? CurrentDate;

            SaveCommand = new Command(async () =>
            {
                CanSave = false;

                DateOnly? deadline = NeedDeadline ? Deadline : null;

                await _goalStorage.SetGoalAsync(Goal, deadline);

                WeakReferenceMessenger.Default.Send<GoalChangedMessage>();

                await PageNavigator.ReturnToPreviousPageAsync();

                CanSave = true;
            });
        }

        public void PrepareView()
        {
            Goal = _goalStorage.Goal;
            Deadline = _goalStorage.Deadline ?? CurrentDate;
        }
    }
}