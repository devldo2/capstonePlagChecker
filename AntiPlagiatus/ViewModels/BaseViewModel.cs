namespace AntiPlagiatus.ViewModels
{
    using AntiPlagiatus.Models.UI;
    using AntiPlagiatus.Providers;
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Windows.ApplicationModel;
    using Windows.Foundation.Metadata;
    using Windows.System;
    using Windows.UI;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;

    public abstract class BaseViewModel : IDisposable, INotifyPropertyChanged
    {
        #region Fields
        private DataTemplate viewTemplate;
        private bool isConnectionMessageShown;
        protected const string InsufficientPermissionsErrorCode = "403";
        protected abstract string ViewTemplateKey { get; }
        protected abstract string ViewKey { get; }

        protected bool IsMobile { get { return Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile"; } }
        public bool IsConnected => InternetConnection.IsConnected;
        public bool IsConnectionMessageShown
        {
            get => isConnectionMessageShown;
            set
            {
                this.isConnectionMessageShown = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion
        public BaseViewModel()
        {
            InternetConnection.InternetConnectionChanged += (sender, e) => SynchronizationContextProvider.UIThreadSyncContext.Post(_ =>
            {
                this.RaisePropertyChanged(() => this.IsConnected);
                this.IsConnectionMessageShown = !e.IsConnected;
            }, null);
        }
        private string DeleteSystemUriSymbols(string data, bool isSubject)
        {
            data = data.Replace("#", "");
            data = data.Replace("&", "");
            if (isSubject)
            {
                data = data.Replace("%", "");
            }

            return data;
        }
        protected virtual async Task<Exception> OnRefresh(object parameter = null)
        {
            Exception exception = null;
            var task = new Task(() =>
            {
                try
                {
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            task.Start();
            await task;
            return exception;
        }
        protected async Task SendFeedback()
        {
            if (this.IsConnected)
            {
                var text = "Your feedback is very important for us!";
                var headerPattern = "{0}-{1}";
                var supportMail = "Ldomashenko6038@conestogac.on.ca";
                var UriDecodedNewLine = "%0d%0a";

                var packageData = new PackageData();
                var appVersion = Package.Current.Id.Version;

                packageData.AppName = Package.Current.DisplayName;
                packageData.Version = string.Format("{0}.{1}.{2}.{3}", appVersion.Major, appVersion.Minor, appVersion.Build, appVersion.Revision);
                packageData.Publisher = Package.Current.PublisherDisplayName;

                var subject = string.Format(headerPattern, packageData.AppName, packageData.Version);
                var body = text + UriDecodedNewLine;

                body += UriDecodedNewLine + UriDecodedNewLine + UriDecodedNewLine + UriDecodedNewLine;

                subject = DeleteSystemUriSymbols(subject, true);
                body = DeleteSystemUriSymbols(body, false);

                var uriForLetter = new Uri("mailto:" + supportMail + "?Subject=" + subject + "&Body=" + body);

                await Launcher.LaunchUriAsync(uriForLetter);
            }
        }
        public virtual void Dispose()
        {
            this.ViewTemplate = null;
        }
        public async Task Refresh(object parameter = null)
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Color.FromArgb(0, 31, 31, 31);
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Color.FromArgb(0, 31, 31, 31);
                    titleBar.ForegroundColor = Colors.White;
                }
            }
            var task = OnRefresh(parameter);
            await task;
            if (task.Result != null)
            {
            }
        }
        #region INotifyProperty implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var handler = this.PropertyChanged;

            if (handler != null)
            {
                var propertyName = GetPropertyName(property);
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", nameof(propertyExpression));
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", nameof(propertyExpression));
            }

            return property.Name;
        }
        #endregion
        public DataTemplate ViewTemplate
        {
            get => this.viewTemplate;
            set
            {
                this.viewTemplate = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
