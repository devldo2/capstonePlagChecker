using AntiPlagiatus.Providers.Interfaces;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System.Profile;

namespace AntiPlagiatus.Providers
{
    public class ShareProvider: IShareProvider
    {
        private bool isRegistered;
        public void Register()
        {
            if (!this.isRegistered)
            {
                var dataTransferManager = DataTransferManager.GetForCurrentView();
                dataTransferManager.DataRequested -= this.OnDataRequested;
                dataTransferManager.DataRequested += this.OnDataRequested;
                this.isRegistered = true;
            }
        }

        public void Share()
        {
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var request = e.Request;
            var deferral = request.GetDeferral();
            var deviceType = AnalyticsInfo.VersionInfo.DeviceFamily;
            var appLink = "HERE WILL BE THE APP LINK FROM STORE WHEN THE APP WILL BE PUBLISHED";

            //var appLinkUri = new Uri(appLink, UriKind.Absolute);
            var messageText = $"Share {Environment.NewLine}{appLink}";

            request.Data.Properties.Title = Package.Current.DisplayName;
            request.Data.SetText(messageText);

            deferral.Complete();
        }
    }
}
