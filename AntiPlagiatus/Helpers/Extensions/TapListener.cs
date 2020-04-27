using System.Windows.Input;
using Windows.UI.Xaml;

namespace AntiPlagiatus.Helpers.Extensions
{

    public static class TapListener
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(TapListener), new PropertyMetadata(null, new PropertyChangedCallback(OnCommandPropertyChanged)));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter", typeof(object), typeof(TapListener), new PropertyMetadata(string.Empty));

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj?.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj?.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj?.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj?.SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var assosiatedFrameworkElement = (FrameworkElement)sender;
            if (e.OldValue != null)
            {
                assosiatedFrameworkElement.Tapped -= OnFrameworkElementTapped;
            }

            if (e.NewValue != null)
            {
                assosiatedFrameworkElement.Tapped += OnFrameworkElementTapped;
            }
        }

        private static void OnFrameworkElementTapped(object sender, RoutedEventArgs e)
        {
            var tappedElement = (FrameworkElement)sender;
            var command = GetCommand(tappedElement);
            if (command != null)
            {
                var parameter = GetCommandParameter(tappedElement);
                if (command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
        }
    }
}
