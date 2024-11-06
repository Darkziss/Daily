using System.Globalization;
using Daily.Tasks;

namespace Daily.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        private static readonly Color _dailyTaskColor;
        private static readonly Color _mandatoryTaskColor;
        private static readonly Color _importantTaskColor;
        private static readonly Color _commonTaskColor;

        private const string dailyTaskColorHex = "edd8c3";
        private const string mandatoryTaskColorHex = "edc3c3";
        private const string importantTaskColorHex = "edebc3";
        private const string commonTaskColorHex = "c3edc4";

        static PriorityToColorConverter()
        {
            _dailyTaskColor = Color.FromRgba(dailyTaskColorHex);
            _mandatoryTaskColor = Color.FromRgba(mandatoryTaskColorHex);
            _importantTaskColor = Color.FromRgba(importantTaskColorHex);
            _commonTaskColor = Color.FromRgba(commonTaskColorHex);
        }

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
            return null;
        }
    }
}