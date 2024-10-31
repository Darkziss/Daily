using System.Globalization;

namespace Daily.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        private static readonly Color incompletedColor = Colors.Transparent;
        private static readonly Color completedColor = Color.FromRgba(completedColorHex);

        private const string completedColorHex = "71b866";
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not bool) return null;

            bool isCompleted = (bool)value;

            return isCompleted ? completedColor : incompletedColor;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not Color) return null;

            Color color = (Color)value;

            return color.Equals(completedColor);
        }
    }
}