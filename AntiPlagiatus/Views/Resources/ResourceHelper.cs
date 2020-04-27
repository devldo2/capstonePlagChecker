using System;
using Windows.UI.Xaml;

namespace AntiPlagiatus.Views.Resources
{
    public static class ResourceHelper
    {
        const string viewTemplateAddress = "ms-appx:///Views/";

        public static DataTemplate GetViewModelTemplate(string viewKey, object templateKey)
        {
            var dictionary = new ResourceDictionary();
            var fullAddress = $"{viewTemplateAddress}/{viewKey}/{viewKey}View.xaml";
            dictionary.Source = new Uri(fullAddress, UriKind.Absolute);
            return dictionary[templateKey] as DataTemplate;
        }
        public static ResourceDictionary GetMainTheme(string theme)
        {
            var dictionary = new ResourceDictionary();
            var fullAddress = $"{viewTemplateAddress}/Resources/Themes/{theme}Theme.xaml";
            dictionary.Source = new Uri(fullAddress, UriKind.Absolute);
            return dictionary;
        }
    }
}
