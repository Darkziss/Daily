using System.Globalization;

namespace Daily.Converters
{
    public class TaskCountToStringConverter : IMultiValueConverter
    {
        public string TaskName { get; set; } = string.Empty;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values[0] is not int || values[1] is not int) return Binding.DoNothing;

            int taskCount = (int)values[0];
            int maxTaskCount = (int)values[1];

            return $"{TaskName} ({taskCount}/{maxTaskCount})";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null!;
        }
    }
}
