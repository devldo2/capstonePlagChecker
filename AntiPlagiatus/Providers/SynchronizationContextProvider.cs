namespace AntiPlagiatus.Providers
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    public static class SynchronizationContextProvider
    {
        private static TaskCompletionSource<bool> initializedTaskSource = new TaskCompletionSource<bool>();

        private static bool isInitialized;
        /*
         * This class using for synchronization in the all application
         * Method Initialize() called in class App.xaml.cs method InitializePhoneApplication()
         */

        /// <summary>
        /// Provides the basic functionality for synchronization
        /// </summary>
        private static SynchronizationContext syncContext;

        /// <summary>
        /// Gets or sets the UI tread sync context.
        /// </summary>
        /// <value>The UI tread sync context.</value>
        public static SynchronizationContext UIThreadSyncContext
        {
            get
            {
                if (!isInitialized)
                {
                    Initialize();
                }

                return syncContext;
            }

            set
            {
                syncContext = value;
            }
        }

        /// <summary>
        /// Initializes the instance. Must be called only in UI Thread
        /// </summary>
        public static void Initialize()
        {
            syncContext = SynchronizationContext.Current;
            if (syncContext == null)
            {
                throw new InvalidOperationException("Initialization must be called only from user interface thread.");
            }

            if (!isInitialized)
            {
                initializedTaskSource.SetResult(true);
                isInitialized = true;
            }
        }

        public static Task PostAsync(Action action)
        {
            return PerformPostAsync(action);
        }

        public static Task PostAsync(Func<Task> asyncFunction)
        {
            return PerformPostAsync(asyncFunction);
        }

        public static async Task PerformPostAsync(object action)
        {
            var taskCompletitionSource = new TaskCompletionSource<bool>();
            await initializedTaskSource.Task;
            syncContext.Post((state) =>
            {
                PerformAction(action, taskCompletitionSource);
            },
            null);

            await taskCompletitionSource.Task;
        }

        private static async void PerformAction(object executedDelegate, TaskCompletionSource<bool> taskCompletitionSource)
        {
            try
            {
                var asyncFunction = executedDelegate as Func<Task>;
                if (asyncFunction != null)
                {
                    await asyncFunction.Invoke();
                }
                else
                {
                    var action = executedDelegate as Action;
                    if (action != null)
                    {
                        action.Invoke();
                    }
                }

                taskCompletitionSource.TrySetResult(true);
            }
            catch (Exception ex)
            {
                taskCompletitionSource.TrySetException(ex);
            }
        }
    }
}
