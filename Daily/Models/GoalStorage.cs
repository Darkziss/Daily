
namespace Daily
{
    public class GoalStorage
    {
        private string _goal = string.Empty;

        public string Goal => _goal;

        public void SetGoal(string goal)
        {
            if (string.IsNullOrWhiteSpace(goal)) return;

            _goal = goal;
        }
    }
}
