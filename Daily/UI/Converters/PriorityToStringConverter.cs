using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class PriorityToStringConverter : IValueConverter
    {
        public string MandatoryTaskText { get; init; } = nameof(TaskPriority.Mandatory);
        public string ImportantTaskText { get; init; } = nameof(TaskPriority.Important);
        public string AdditionalTaskText { get; init; } = nameof(TaskPriority.Additional);

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not TaskPriority) return null;

            TaskPriority priority = (TaskPriority)value;

            return priority switch
            {
                TaskPriority.Mandatory => MandatoryTaskText,
                TaskPriority.Important => ImportantTaskText,
                TaskPriority.Additional => AdditionalTaskText,
                _ => string.Empty
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}