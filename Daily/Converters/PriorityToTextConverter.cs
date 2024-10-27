using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class PriorityToTextConverter : IValueConverter
    {
        private const string dailyTaskText = "Ежедневная";
        private const string mandatoryTaskText = "Обязательная";
        private const string importantTaskText = "Важная";
        private const string commonTaskText = "Обычная";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not TaskPriority) return null;

            TaskPriority priority = (TaskPriority)value;

            return priority switch
            {
                TaskPriority.Daily => dailyTaskText,
                TaskPriority.Mandatory => mandatoryTaskText,
                TaskPriority.Important => importantTaskText,
                TaskPriority.Common => commonTaskText,
                _ => dailyTaskText
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not string) return null;

            string text = (string)value;

            return text switch
            {
                dailyTaskText => TaskPriority.Daily,
                mandatoryTaskText => TaskPriority.Mandatory,
                importantTaskText => TaskPriority.Important,
                commonTaskText => TaskPriority.Common,
                _ => TaskPriority.Daily
            };
        }
    }
}