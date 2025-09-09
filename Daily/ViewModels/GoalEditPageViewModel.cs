using Daily.Tasks;
using Daily.Navigation;
using Daily.Messages;
using Daily.Toasts;
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

        public DateOnly MinimumDeadlineDate => _goalStorage.MinimumDeadlineDate;

        public Command SaveCommand { get; }

        private bool IsGoalFilled => !string.IsNullOrWhiteSpace(_goal);

        public GoalEditPageViewModel(GoalStorage goalStorage)
        {
            _goalStorage = goalStorage;

            _goal = goalStorage.Goal;
            _deadline = _goalStorage.Deadline ?? MinimumDeadlineDate;

            SaveCommand = new Command(async () =>
            {
                CanSave = false;

                if (NeedDeadline && Deadline < MinimumDeadlineDate) await GoalToastHandler.ShowDeadlineDateErrorToastAsync();
                else
                {
                    string? goal = IsGoalFilled ? Goal : null;
                    DateOnly? deadline = IsGoalFilled && NeedDeadline ? Deadline : null;

                    await _goalStorage.SetGoalAsync(goal, deadline);

                    WeakReferenceMessenger.Default.Send<GoalChangedMessage>();

                    await PageNavigator.ReturnToPreviousPageAsync();
                }

                CanSave = true;
            });
        }

        public void PrepareView()
        {
            Goal = _goalStorage.Goal;

            Deadline = _goalStorage.Deadline ?? MinimumDeadlineDate;
            NeedDeadline = _goalStorage.Deadline.HasValue;

            OnPropertyChanged(nameof(MinimumDeadlineDate));
        }
    }
}