using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class GoalStatusToIsEnabledConverter : IValueConverter
    {
        private const GoalStatus DisabledGoalStatus = GoalStatus.None;
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not GoalStatus) return null;

            GoalStatus status = (GoalStatus)value;

            if (status == DisabledGoalStatus) return false;
            else return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}