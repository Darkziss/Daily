using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public Color DailyColor { get; set; } = Colors.Orange;
        public Color MandatoryColor { get; set; } = Colors.Red;
        public Color ImportantColor { get; set; } = Colors.Yellow;
        public Color CommonColor { get; set; } = Colors.Green;
 
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not TaskPriority) return null;

            TaskPriority priority = (TaskPriority)value;

            return priority switch
            {
                TaskPriority.Daily => DailyColor,
                TaskPriority.Mandatory => MandatoryColor,
                TaskPriority.Important => ImportantColor,
                TaskPriority.Common => CommonColor,
                _ => null
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}