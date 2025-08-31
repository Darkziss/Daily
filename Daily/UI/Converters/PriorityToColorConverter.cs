using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public Color DailyColor { get; init; } = Colors.Orange;
        public Color MandatoryColor { get; init; } = Colors.Red;
        public Color ImportantColor { get; init; } = Colors.Yellow;
        public Color AdditionalColor { get; init; } = Colors.Green;
 
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not TaskPriority) return null;

            TaskPriority priority = (TaskPriority)value;

            return priority switch
            {
                TaskPriority.Daily => DailyColor,
                TaskPriority.Mandatory => MandatoryColor,
                TaskPriority.Important => ImportantColor,
                TaskPriority.Additional => AdditionalColor,
                _ => null
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}