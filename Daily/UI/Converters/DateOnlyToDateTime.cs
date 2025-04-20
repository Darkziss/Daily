using System.Globalization;

namespace Daily.Converters
{
    public class DateOnlyToDateTime : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            DateOnly? dateOnly = (DateOnly?)value;

            if (dateOnly.HasValue) return dateOnly.Value.ToDateTime(TimeOnly.MinValue);
            else return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            DateTime? dateTime = (DateTime?)value;

            if (dateTime.HasValue) return DateOnly.FromDateTime(dateTime.Value);
            else return null;
        }
    }
}