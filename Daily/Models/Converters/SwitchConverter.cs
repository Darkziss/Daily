using System.Globalization;

namespace Daily
{
    public class SwitchConverter : IValueConverter
    {
        public object? Default { get; set; }
        public List<Case> Cases { get; set; }

        public SwitchConverter()
        {
            Cases = new List<Case>();
        }
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;
            
            Case? pair = null;

            foreach (var c in Cases)
            {
                if (value.Equals(c.Key)) pair = c;
            }

            return pair == null ? Default : pair.Value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;
            
            Case? pair = null;

            foreach (var c in Cases)
            {
                if (value.Equals(c.Value)) pair = c;
            }

            return pair == null ? Default : pair.Value;
        }
    }
}