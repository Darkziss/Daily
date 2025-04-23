using System.Globalization;
using System.Diagnostics;
using System;

namespace Daily.Converters
{
    public class DateOnlyToDateTime : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            DateOnly? dateOnly = (DateOnly?)value;

            Debug.WriteLine($"DATEONLY: {dateOnly:d}");

            if (dateOnly.HasValue) return dateOnly.Value.ToDateTime(TimeOnly.MinValue);
            else return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            DateTime? dateTime = (DateTime?)value;

            Debug.WriteLine($"DATETIME: {dateTime:d}");

            if (dateTime.HasValue) return DateOnly.FromDateTime(dateTime.Value);
            else return null;
        }
    }
}