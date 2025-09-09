using System.Globalization;

namespace Daily.Converters
{
    public class BoolToTextDecorationsConverter : IValueConverter
    {
        public TextDecorations TrueTextDecorations { get; set; }

        public TextDecorations FalseTextDecorations { get; set; }
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not bool) return null;

            return (bool)value ? TrueTextDecorations : FalseTextDecorations;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}