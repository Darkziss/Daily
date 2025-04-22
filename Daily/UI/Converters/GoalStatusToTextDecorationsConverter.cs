using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class GoalStatusToTextDecorationsConverter : IValueConverter
    {
        public TextDecorations IncompletedTextDecorations { get; init; } = TextDecorations.None;
        public TextDecorations CompletedTextDecorations { get; init; } = TextDecorations.Strikethrough;
        public TextDecorations OverdueTextDecorations { get; init; } = TextDecorations.None;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not GoalStatus) return null;
            
            GoalStatus status = (GoalStatus)value;

            return status switch
            {
                GoalStatus.Incompleted => IncompletedTextDecorations,
                GoalStatus.Completed => CompletedTextDecorations,
                GoalStatus.Overdue => OverdueTextDecorations,
                _ => null
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}