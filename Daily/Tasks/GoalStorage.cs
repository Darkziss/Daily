using Daily.Data;

namespace Daily.Tasks
{
    public class GoalStorage
    {
        private Goal? _goal;

        private readonly DataProvider _dataProvider;

        public string? Goal => _goal?.Text;
        public DateOnly? Deadline => _goal?.Deadline;

        public GoalStatus? Status => _goal?.Status;

        public bool IsNone => _goal?.Status == GoalStatus.None;

        public bool IsCompleted => _goal?.Status == GoalStatus.Completed;

        public DateOnly MinimumDeadlineDate => DateOnly.FromDateTime(DateTime.Now).AddDays(1);

        public GoalStorage(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<Goal> LoadGoalAsync()
        {
            Goal? goal = await _dataProvider.LoadGoalAsync();

            _goal = goal ?? new();

            if (!IsCompleted) RefreshOverdueStatus();

            return _goal;
        }

        public async Task SetGoalAsync(string? goal, DateOnly? deadline)
        {
            if (deadline < MinimumDeadlineDate) throw new ArgumentException(nameof(deadline));
            
            _goal.Text = goal?.Trim();
            _goal.Deadline = deadline;

            _goal.Status = _goal.Text == null ? GoalStatus.None : GoalStatus.Incompleted;

            await _dataProvider.SaveGoalAsync(_goal);
        }

        public async Task CompleteGoalAsync()
        {
            if (IsCompleted) throw new InvalidOperationException(nameof(IsCompleted));

            _goal.Status = GoalStatus.Completed;

            await _dataProvider.SaveGoalAsync(_goal);
        }

        public async Task ResetGoalStatusAsync()
        {
            if (!IsCompleted) return;

            _goal.Status = CheckForOverdueNow() ? GoalStatus.Overdue : GoalStatus.Incompleted;

            await _dataProvider.SaveGoalAsync(_goal);
        }

        public void RefreshOverdueStatus()
        {
            if (IsCompleted) throw new InvalidOperationException(nameof(IsCompleted));

            if (CheckForOverdueNow()) _goal.Status = GoalStatus.Overdue;
        }

        private bool CheckForOverdueNow() => _goal.Deadline <= DateOnly.FromDateTime(DateTime.Now);
    }
}