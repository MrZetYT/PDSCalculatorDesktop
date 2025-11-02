using System;
using System.Globalization;
using System.Windows.Data;
using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Converters
{
    public class MeasurementTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MeasurementType measurementType)
            {
                return measurementType switch
                {
                    MeasurementType.PDK => "Предельно допустимая концентрация вещества в контрольном створе",
                    MeasurementType.KNK => "Коэффициент неконсервативности вещества)",
                    MeasurementType.BackgroundConcentration => "Фоновая концентрация вещества в контрольном створе",
                    MeasurementType.SubstanceConcentration => "Концентрация вещества в сточных водах выпуска",
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
