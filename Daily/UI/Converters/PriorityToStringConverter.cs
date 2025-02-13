using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class PriorityToStringConverter : IValueConverter
    {
        public string DailyTaskText { get; init; } = nameof(TaskPriority.Daily);
        public string MandatoryTaskText { get; init; } = nameof(TaskPriority.Mandatory);
        public string ImportantTaskText { get; init; } = nameof(TaskPriority.Important);
        public string CommonTaskText { get; init; } = nameof(TaskPriority.Common);

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not TaskPriority) return null;

            TaskPriority priority = (TaskPriority)value;

            return priority switch
            {
                TaskPriority.Daily => DailyTaskText,
                TaskPriority.Mandatory => MandatoryTaskText,
                TaskPriority.Important => ImportantTaskText,
                TaskPriority.Common => CommonTaskText,
                _ => string.Empty
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}