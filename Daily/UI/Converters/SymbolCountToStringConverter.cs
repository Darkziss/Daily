using System.Globalization;

namespace Daily.Converters
{
    public class SymbolCountToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null || value is not int || parameter is null) return null!;
            
            int symbolCount = (int)value;
            int maxSymbolCount = (int)parameter;

            return $"{symbolCount}/{maxSymbolCount} символов";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null!;
        }
    }
}
