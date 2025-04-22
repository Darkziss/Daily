using Daily.Data;

namespace Daily.Tasks
{
    public class GoalStorage
    {
        private readonly Goal _goal;

        private readonly DataProvider _dataProvider;

        public string? Goal => _goal.Text;
        public DateOnly? Deadline => _goal.Deadline;

        public GoalStatus Status => _goal.Status;

        public bool IsCompleted => _goal.Status == GoalStatus.Completed;
        private bool IsOverdue => _goal.Deadline <= DateOnly.FromDateTime(DateTime.Now);

        public GoalStorage(DataProvider dataProvider)
        {
            _goal = dataProvider.Goal ?? new Goal();

            if (IsOverdue) _goal.Status = GoalStatus.Overdue;

            _dataProvider = dataProvider;
        }

        public async Task SetGoalAsync(string? goal, DateOnly? deadline)
        {
            _goal.Text = goal?.Trim();
            _goal.Deadline = deadline;

            _goal.Status = IsOverdue ? GoalStatus.Overdue : GoalStatus.Incompleted;

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

            _goal.Status = IsOverdue ? GoalStatus.Overdue : GoalStatus.Incompleted;

            await _dataProvider.SaveGoalAsync(_goal);
        }
    }
}