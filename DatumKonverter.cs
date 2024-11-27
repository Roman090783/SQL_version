using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SQL_version
{
    public class DatumKonverter : IValueConverter
    {
        // Implementierung der IValueConverter-Methoden
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object retValue = null;
            var notizDatum = (DateTime)value;
            string para = (string)parameter;

            if (targetType == typeof(string))
            {
                retValue = notizDatum.ToShortDateString();
            }
            else if (targetType == typeof(Brush))
            {
                retValue = para == "vordergrund" ? notizDatum < DateTime.Today ? Brushes.DarkBlue : Brushes.Red : para == "hintergrund" ? notizDatum < DateTime.Today ? Brushes.Orange : Brushes.LightGreen : null;
            }

            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
