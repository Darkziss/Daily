using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        private double incompletedOpacity = 1d;
        private double completedOpacity = 0.5d;
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not bool) return null;

            bool isCompleted = (bool)value;

            return isCompleted ? completedOpacity : incompletedOpacity;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not double) return null;

            double opacity = (double)value;

            return opacity == completedOpacity;
        }
    }
}