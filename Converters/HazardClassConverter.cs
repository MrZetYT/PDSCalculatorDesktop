using System;
using System.Globalization;
using System.Windows.Data;
using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Converters
{
    public class HazardClassConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HazardClass hazardClass)
            {
                return hazardClass switch
                {
                    HazardClass.ExtremelyHazardous => "Чрезвычайно опасный (I класс)",
                    HazardClass.HighlyHazardous => "Высоко опасный (II класс)",
                    HazardClass.Hazardous => "Опасный (III класс)",
                    HazardClass.ModeratelyHazardous => "Умеренно опасный (IV класс)",
                    _ => value.ToString()
                };
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}