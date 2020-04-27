using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AntiPlagiatus.Helpers.Converters
{
    public class EmptyListVisibilityConverter : IValueConverter
    {
        private const string invertParameter = "invert";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = true;

            if (value != null)
            {
                ICollection list = value as ICollection;
                if (list != null)
                {
                    if (list.Count != 0)
                        result = false;
                }
            }

            var stringParameter = parameter as string;
            if (!string.IsNullOrEmpty(stringParameter) && stringParameter == invertParameter)
                result = !result;

            return result ? Visibility.Visible : Visibility.Collapsed;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
