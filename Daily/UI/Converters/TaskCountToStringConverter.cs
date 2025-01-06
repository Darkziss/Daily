using System.Globalization;

namespace Daily.Converters
{
    public class TaskCountToStringConverter : IValueConverter
    {
        public string TaskName { get; set; } = string.Empty;
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null || value is not int || parameter is null || parameter is not int) return null!;

            int taskCount = (int)value;
            int maxTaskCount = (int)parameter;

            return $"{TaskName} ({taskCount}/{maxTaskCount})";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null!;
        }
    }
}
