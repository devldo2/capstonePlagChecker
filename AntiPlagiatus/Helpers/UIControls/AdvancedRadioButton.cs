using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AntiPlagiatus.Helpers.UIControls
{
    public class AdvancedRadioButton: RadioButton
    {
        public SolidColorBrush CheckedBgBrush
        {
            get { return (SolidColorBrush)GetValue(CheckedBgBrushProperty); }
            set { SetValue(CheckedBgBrushProperty, value); }
        }

        public static readonly DependencyProperty CheckedBgBrushProperty =
            DependencyProperty.Register("CheckedBgBrush", typeof(SolidColorBrush), typeof(AdvancedRadioButton), new PropertyMetadata(Colors.Transparent));
    }
}
