using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AntiPlagiatus.Helpers.Extensions
{
    public static class ListItemClickListener
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof(ICommand), typeof(ListItemClickListener), new PropertyMetadata(null, OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter", typeof(object), typeof(ListItemClickListener), new PropertyMetadata(null));

        public static readonly DependencyProperty PropertyPathParameter = DependencyProperty.RegisterAttached(
            "PropertyPath", typeof(string), typeof(ListItemClickListener), new PropertyMetadata(null));

        public static string GetPropertyPath(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyPathParameter);
        }

        public static void SetPropertyPath(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyPathParameter, value);
        }

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var listViewBase = sender as ListViewBase;
            if (listViewBase != null)
            {
                if (e.OldValue != null)
                {
                    listViewBase.ItemClick -= ListItemClick;
                }

                if (e.NewValue != null)
                {
                    listViewBase.ItemClick += ListItemClick;
                }
            }
        }

        private static void ListItemClick(object sender, ItemClickEventArgs e)
        {
            var ansector = e.OriginalSource as DependencyObject;
            if (ansector != null)
            {
                var tappedObject = sender as DependencyObject;
                CallCommand(tappedObject, ansector, e.ClickedItem);
            }
        }

        private static void CallCommand(DependencyObject tappedObject, DependencyObject ansector, object clickedItem)
        {
            var itemClickCommand = GetCommand(ansector);
            if (itemClickCommand != null)
            {
                var path = GetPropertyPath(ansector);
                if (!string.IsNullOrEmpty(path))
                {
                    var listViewBase = ansector as ListViewBase;
                    if (listViewBase != null)
                    {
                        var binding = new Binding();
                        binding.Path = new PropertyPath(path);
                        binding.Source = clickedItem;
                        listViewBase.SetBinding(CommandParameterProperty, binding);
                    }
                }

                var parameter = GetCommandParameter(ansector) ?? clickedItem;
                if (itemClickCommand.CanExecute(parameter))
                {
                    if (ansector == tappedObject)
                    {
                        itemClickCommand.Execute(parameter);
                    }
                }
            }
        }
    }
}
