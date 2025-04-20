
namespace Daily.Tasks
{
    public class Goal
    {
        public string? Text { get; set; } = null;
        public DateOnly? Deadline { get; set; } = null;

        public bool IsCompleted { get; set; } = false;
    }
}