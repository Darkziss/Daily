
namespace Daily.Tasks
{
    public class Goal
    {
        public string Text { get; set; } = string.Empty;

        public DateOnly? Deadline { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}