using Daily.Tasks;
using Daily.Navigation;
using Daily.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Daily.ViewModels
{
    public partial class GoalEditPageViewModel : ObservableObject
    {
        [ObservableProperty] private string? _goal;
        [ObservableProperty] private DateOnly _deadline;

        [ObservableProperty] private bool _needDeadline = false;

        [ObservableProperty] private bool _canSave = true;

        private readonly GoalStorage _goalStorage;

        public Command SaveCommand { get; }

        private bool IsGoalFilled => !string.IsNullOrWhiteSpace(_goal);

        private static DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.Now);

        public GoalEditPageViewModel(GoalStorage goalStorage)
        {
            _goalStorage = goalStorage;

            _goal = goalStorage.Goal;
            _deadline = _goalStorage.Deadline ?? CurrentDate;

            SaveCommand = new Command(async () =>
            {
                CanSave = false;

                string? goal = IsGoalFilled ? Goal : null;
                DateOnly? deadline = IsGoalFilled && NeedDeadline ? Deadline : null;

                await _goalStorage.SetGoalAsync(goal, deadline);

                WeakReferenceMessenger.Default.Send<GoalChangedMessage>();

                await PageNavigator.ReturnToPreviousPageAsync();

                CanSave = true;
            });
        }

        public void PrepareView()
        {
            Goal = _goalStorage.Goal;

            Deadline = _goalStorage.Deadline ?? CurrentDate;
            NeedDeadline = _goalStorage.Deadline.HasValue;
        }
    }
}