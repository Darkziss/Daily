using Daily.Data;

namespace Daily.Tasks
{
    public class GoalStorage
    {
        private readonly Goal _goal;

        private readonly DataProvider _dataProvider;

        public string? Goal => _goal.Text;
        public DateOnly? Deadline => _goal.Deadline;

        public bool IsCompleted => _goal.IsCompleted;

        public GoalStorage(DataProvider dataProvider)
        {
            _goal = dataProvider.Goal ?? new Goal();

            _dataProvider = dataProvider;
        }

        public async Task SetGoalAsync(string? goal, DateOnly? deadline)
        {
            _goal.Text = goal?.Trim();
            _goal.Deadline = deadline;

            await _dataProvider.SaveGoalAsync(_goal);
        }

        public async Task CompleteGoalAsync()
        {
            if (IsCompleted) throw new InvalidOperationException(nameof(IsCompleted));

            _goal.IsCompleted = true;

            await _dataProvider.SaveGoalAsync(_goal);
        }

        public async Task ResetGoalStatusAsync()
        {
            if (!IsCompleted) return;

            _goal.IsCompleted = false;

            await _dataProvider.SaveGoalAsync(_goal);
        }
    }
}