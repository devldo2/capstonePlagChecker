namespace AntiPlagiatus.Helpers.Extensions
{
    using System.Threading.Tasks;
    using global::Windows.UI.Xaml;

    /// <summary>
    /// Contains extension methods to wait for FrameworkElement events.
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Waits for the element to load (construct and add to the main object tree).
        /// </summary>
        public static async Task WaitForLoadedAsync(this FrameworkElement frameworkElement)
        {
            await EventAsync.FromRoutedEvent(eh => frameworkElement.Loaded += eh, eh => frameworkElement.Loaded -= eh);
        }

        /// <summary>
        /// Waits for the element to unload (disconnect from the main object tree).
        /// </summary>
        public static async Task WaitForUnloadedAsync(this FrameworkElement frameworkElement)
        {
            await EventAsync.FromRoutedEvent(eh => frameworkElement.Unloaded += eh, eh => frameworkElement.Unloaded -= eh);
        }

        /// <summary>
        /// Waits for the next layout update event.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <returns></returns>
        public static async Task WaitForLayoutUpdateAsync(this FrameworkElement frameworkElement)
        {
            await EventAsync.FromEvent<object>(eh => frameworkElement.LayoutUpdated += eh, eh => frameworkElement.LayoutUpdated -= eh);
        }
    }
}
