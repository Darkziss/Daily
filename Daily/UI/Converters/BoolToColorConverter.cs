using System.Diagnostics;
using System.Globalization;

namespace Daily.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; } = Colors.White;
        public Color FalseColor { get; set; } = Colors.Gray;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not bool) return null;

            Debug.WriteLine($"TrueColor: {TrueColor.ToRgbaHex()}");
            Debug.WriteLine($"FalseColor: {FalseColor.ToRgbaHex()}");

            bool canEditTask = (bool)value;

            return canEditTask ? TrueColor : FalseColor;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}