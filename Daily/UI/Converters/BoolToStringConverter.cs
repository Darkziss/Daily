using System.Globalization;

namespace Daily.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public string? TrueString { get; set; } = "True";
        public string? FalseString { get; set; } = "False";
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not bool) return null;

            return (bool)value ? TrueString : FalseString;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
