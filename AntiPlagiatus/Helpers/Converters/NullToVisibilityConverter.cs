using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AntiPlagiatus.Helpers.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        private const string InvertParameter = "invert";

        public object Convert(object value, Type targetType, object parameter, string language)
        {

            var objValue = (object)value;
            //var stringParameter = parameter as string;

            //if (!string.IsNullOrEmpty(stringParameter) && stringParameter == InvertParameter)
            //{
                
            //}

            if(objValue != null)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
