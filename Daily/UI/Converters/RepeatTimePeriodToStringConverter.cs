using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class RepeatTimePeriodToStringConverter : IValueConverter
    {
        public string DayRepeatTimeText { get; init; } = string.Empty;
        public string WeekRepeatTimeText { get; init; } = string.Empty;
        public string MonthRepeatTimeText { get; init; } = string.Empty;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not TaskRepeatTimePeriod)
                return null;

            TaskRepeatTimePeriod period = (TaskRepeatTimePeriod)value;

            return period switch
            {
                TaskRepeatTimePeriod.Day => DayRepeatTimeText,
                TaskRepeatTimePeriod.Week => WeekRepeatTimeText,
                TaskRepeatTimePeriod.Month => MonthRepeatTimeText,
                _ => string.Empty
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}