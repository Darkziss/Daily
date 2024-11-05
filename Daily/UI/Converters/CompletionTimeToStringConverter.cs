using System.Diagnostics;
using System.Globalization;

namespace Daily.Converters
{
    public class CompletionTimeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not int) return 0;

            int time = (int)value;

            return time.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not string) return 0;

            string text = (string)value;

            if (int.TryParse(text, out int time)) return time;
            else return 0;
        }
    }
}