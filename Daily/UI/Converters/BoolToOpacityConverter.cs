using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public double TrueOpacity { get; init; } = 0.5d;
        public double FalseOpacity { get; init; } = 1d;
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not bool) return null;

            bool isCompleted = (bool)value;

            return isCompleted ? TrueOpacity : FalseOpacity;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}