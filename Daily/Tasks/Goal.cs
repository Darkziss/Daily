
namespace Daily.Tasks
{
    public class Goal
    {
        public string? Text { get; set; }
        public DateOnly? Deadline { get; set; }

        public GoalStatus Status { get; set; }
    }
}