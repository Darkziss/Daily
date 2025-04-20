using Daily.Data;

namespace Daily.Tasks
{
    public class GoalStorage
    {
        private readonly Goal _goal;

        private readonly DataProvider _dataProvider;

        public string Goal => _goal.Text;
        public DateOnly? Deadline => _goal.Deadline;

        public GoalStorage(DataProvider dataProvider)
        {
            _goal = dataProvider.Goal ?? new Goal();

            _dataProvider = dataProvider;
        }

        public bool IsSameGoal(string goal)
        {
            return Goal.Equals(goal);
        }

        public async Task SetGoalAsync(string goal, DateOnly? deadline)
        {
            _goal.Text = goal.Trim();
            _goal.Deadline = deadline;

            await _dataProvider.SaveGoalAsync(_goal);
        }
    }
}