using System.Diagnostics;
using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class RepeatTimePeriodConverter : IMultiValueConverter
    {
        private const string dayRepeatTime = "день";
        private const string weekRepeatTime = "неделя";
        private const string monthRepeatTime = "месяц";
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool ValidateValues()
            {
                if (values == null) return false;
                else if (values[0] is not int || values[1] is not TaskRepeatTimePeriod) return false;
                else return true;
            }

            if (!ValidateValues()) return BindableProperty.UnsetValue;

            int targetRepeatCount = (int)values[0];
            TaskRepeatTimePeriod repeatTimePeriod = (TaskRepeatTimePeriod)values[1];

            string periodName = GetTextByRepeatTimePeriod(repeatTimePeriod);

            return $"{targetRepeatCount} р/{periodName}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null!;
        }

        private string GetTextByRepeatTimePeriod(TaskRepeatTimePeriod period)
        {
            return period switch
            {
                TaskRepeatTimePeriod.Day => dayRepeatTime,
                TaskRepeatTimePeriod.Week => weekRepeatTime,
                TaskRepeatTimePeriod.Month => monthRepeatTime,
                _ => throw new NotImplementedException()
            };
        }
    }
}