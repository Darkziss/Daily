using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class GoalStatusToColorConverter : IValueConverter
    {
        public Color IncompletedColor { get; init; } = Colors.White;
        public Color CompletedColor { get; init; } = Colors.Green;
        public Color OverdueColor { get; init; } = Colors.Red;
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not GoalStatus) return null;

            GoalStatus status = (GoalStatus)value;

            return status switch
            {
                GoalStatus.None => IncompletedColor,
                GoalStatus.Incompleted => IncompletedColor,
                GoalStatus.Completed => CompletedColor,
                GoalStatus.Overdue => OverdueColor,
                _ => null
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}