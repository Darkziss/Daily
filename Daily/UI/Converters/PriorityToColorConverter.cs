using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        private static readonly Color _dailyTaskColor = Color.FromRgba(dailyTaskColorHex);
        private static readonly Color _mandatoryTaskColor = Color.FromRgba(mandatoryTaskColorHex);
        private static readonly Color _importantTaskColor = Color.FromRgba(importantTaskColorHex);
        private static readonly Color _commonTaskColor = Color.FromRgba(commonTaskColorHex);

        private const string dailyTaskColorHex = "edd8c3";
        private const string mandatoryTaskColorHex = "edc3c3";
        private const string importantTaskColorHex = "edebc3";
        private const string commonTaskColorHex = "c3edc4";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not TaskPriority) return null;

            TaskPriority priority = (TaskPriority)value;

            return priority switch
            {
                TaskPriority.Daily => _dailyTaskColor,
                TaskPriority.Mandatory => _mandatoryTaskColor,
                TaskPriority.Important => _importantTaskColor,
                TaskPriority.Common => _commonTaskColor,
                _ => _dailyTaskColor
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not Color) return null;

            Color color = (Color)value;
            string hex = color.ToRgbaHex();

            return hex switch
            {
                dailyTaskColorHex => TaskPriority.Daily,
                mandatoryTaskColorHex => TaskPriority.Mandatory,
                importantTaskColorHex => TaskPriority.Important,
                commonTaskColorHex => TaskPriority.Common,
                _ => TaskPriority.Daily
            };
        }
    }
}