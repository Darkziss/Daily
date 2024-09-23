using Daily.Data;

namespace Daily
{
    public class GoalStorage
    {
        private string _goal;

        private readonly DataProvider _dataProvider;

        public string Goal => _goal;

        public GoalStorage(DataProvider dataProvider)
        {
            _goal = dataProvider.LoadGoal();

            _dataProvider = dataProvider;
        }

        public bool IsSameGoal(string goal)
        {
            return _goal.Equals(goal);
        }

        public async Task SetGoalAsync(string goal)
        {
            _goal = goal;

            await _dataProvider.SaveGoalAsync(goal);
        }
    }
}