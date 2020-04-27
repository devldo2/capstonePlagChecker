using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AntiPlagiatus.Helpers.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private const string InvertParameter = "invert";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var booleanValue = (bool)value;
            var stringParameter = parameter as string;

            if (!string.IsNullOrEmpty(stringParameter) && stringParameter == InvertParameter)
            {
                booleanValue = !booleanValue;
            }

            return booleanValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }

    public class VisibilityToBooleanConverter : IValueConverter
    {
        private const string InvertParameter = "invert";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var booleanValue = (Visibility)value == Visibility.Visible;
            if ((string)parameter == InvertParameter)
            {
                booleanValue = !booleanValue;
            }

            return booleanValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var result = Visibility.Collapsed;
            var booleanValue = (bool)value;
            if ((string)parameter == InvertParameter)
            {
                booleanValue = !booleanValue;
            }

            if (booleanValue)
            {
                result = Visibility.Visible;
            }
            return result;
        }
    }

    public class EmptyStringToBooleanConverter : IValueConverter
    {
        private const string invertParameter = "invert";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = false;
            var inputValue = value as string;
            var stringParameter = parameter as string;

            result = string.IsNullOrEmpty(inputValue);

            if (!string.IsNullOrEmpty(stringParameter) && stringParameter == invertParameter)
            {
                result = !result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        private const string invertParameter = "invert";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = false;
            var inputValue = value as string;
            var stringParameter = parameter as string;

            result = string.IsNullOrEmpty(inputValue);

            if (!string.IsNullOrEmpty(stringParameter) && stringParameter == invertParameter)
            {
                result = !result;
            }

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
