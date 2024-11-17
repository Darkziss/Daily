using System.Globalization;

namespace Daily.Converters
{
    public class TaskProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return null!;
            else if (values[0] is not int) return null!;
            else if (values[1] is not int) return null!;

            int repeatCount = (int)values[0];
            int targetRepeatCount = (int)values[1];

            return $"{repeatCount}/{targetRepeatCount}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null!;
        }
    }
}