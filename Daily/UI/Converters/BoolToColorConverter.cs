using System.Globalization;

namespace Daily.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public Color TrueColor { get; init; } = Colors.White;
        public Color FalseColor { get; init; } = Colors.Gray;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not bool) return null;

            return (bool)value ? TrueColor : FalseColor;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}