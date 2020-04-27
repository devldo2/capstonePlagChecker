using AntiPlagiatus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AntiPlagiatus.Helpers.Converters
{
    public class SectionTypeToBooleanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SectionType inputType;
            return (Enum.TryParse(value.ToString(), out inputType) && inputType.ToString().Equals(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            object result = DependencyProperty.UnsetValue;
            if (value.Equals(true))
            {
                SectionType outputType;
                if (Enum.TryParse(parameter.ToString(), out outputType))
                    result = outputType;
            }
            return result;
        }
    }
}
