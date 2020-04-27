using AntiPlagiatus.Helpers;
using AntiPlagiatus.Models;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Views.Resources;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;

namespace AntiPlagiatus.ViewModels
{
    public class RateRaviewViewModel : BaseViewModel, IVisible
    {
        private const int IsTimeToShow = 10;
        private const string IsProhibitAskRateKey = "IsProhibitAskRate";
        private const string IsTimeToShowKey = "IsTimeToShow";
        private bool isVisible;
        private int ratingValue;
        public RateRaviewViewModel()
        {
            this.RateCommand = new Command(this.RateCommand_Execute);
            this.RateLaterCommand = new Command(this.RateLaterCommand_Execute);
            var task = Refresh();
        }
        protected override string ViewTemplateKey => "ViewTemplate";
        protected override string ViewKey => "RateReview";
        public Command RateCommand { get; private set; }
        public Command RateLaterCommand { get; private set; }
        protected async override Task<Exception> OnRefresh(object parameter = null)
        {
            Exception exception = null;
            try
            {
                this.ViewTemplate = ResourceHelper.GetViewModelTemplate(this.ViewKey, ViewTemplateKey);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return exception;
        }
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value != this.isVisible)
                {
                    this.isVisible = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public int RatingValue
        {
            get => this.ratingValue;
            set
            {
                this.ratingValue = value;
                this.RaisePropertyChanged();
            }
        }
        private async void OnRatingValueChanged()
        {
            if (this.RatingValue > 3)
            {
                this.RedirectToStore();
            }
            else
            {
                await SendFeedback();
            }
        }
        private void Closing()
        {
            this.IsVisible = false;
            this.RatingValue = 0;
        }
        private async void RedirectToStore()
        {
            if (this.IsConnected)
            {
                var rateAndReviewUri = String.Format("ms-windows-store:REVIEW?PFN={0}", Package.Current.Id.FamilyName);
                await Launcher.LaunchUriAsync(new Uri(rateAndReviewUri));
                this.ProhibitAskRate();

                this.Closing();
            }
        }
        private void ProhibitAskRate()
        {
            SettingsProvider.WriteValueIntoAppData<bool>(IsProhibitAskRateKey, true);
        }
        private bool IsRateControlVisible()
        {
            bool result = false;
            var isProhibitAskRate = SettingsProvider.ReadValueFromAppData<bool>(IsProhibitAskRateKey);
            var isTimeToShow = SettingsProvider.ReadValueFromAppData<int>(IsTimeToShowKey);

            if (!isProhibitAskRate)
            {
                if (isTimeToShow < IsTimeToShow) SettingsProvider.WriteValueIntoAppData<int>(IsTimeToShowKey, ++isTimeToShow);
                else if (isTimeToShow == IsTimeToShow)
                {
                    SettingsProvider.WriteValueIntoAppData<int>(IsTimeToShowKey, 0);
                    result = true;
                }
            }

            return result;
        }
        #region Command Handlers
        private void RateLaterCommand_Execute()
        {
            this.Closing();
        }
        private void RateCommand_Execute()
        {
            this.OnRatingValueChanged();
        }
        public void CheckRateReview()
        {
            this.IsVisible = this.IsRateControlVisible();
        }
        #endregion
    }
}
