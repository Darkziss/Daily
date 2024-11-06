using System.Globalization;

namespace Daily.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        private static readonly Color incompletedColor;
        private static readonly Color completedColor;

        private const string completedColorHex = "71b866";

        static BoolToColorConverter()
        {
            incompletedColor = Colors.Transparent;
            completedColor = Color.FromRgba(completedColorHex);
        }
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not bool) return null;

            bool isCompleted = (bool)value;

            return isCompleted ? completedColor : incompletedColor;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}