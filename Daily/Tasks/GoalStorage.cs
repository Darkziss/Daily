using Daily.Data;

namespace Daily.Tasks
{
    public class GoalStorage
    {
        private readonly DataProvider _dataProvider;

        public string Goal { get; set; }

        public GoalStorage(DataProvider dataProvider)
        {
            Goal = dataProvider.Goal ?? string.Empty;

            _dataProvider = dataProvider;
        }

        public bool IsSameGoal(string goal)
        {
            return Goal.Equals(goal);
        }

        public async Task SetGoalAsync(string goal)
        {
            Goal = goal;

            await _dataProvider.SaveGoalAsync(goal);
        }
    }
}