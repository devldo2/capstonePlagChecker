using AntiPlagiatus.Helpers;
using AntiPlagiatus.Models;
using AntiPlagiatus.Models.Inferfaces;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Providers.Interfaces;
using AntiPlagiatus.Views.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace AntiPlagiatus.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISectionManager sectionManager;
        private readonly IAccountProvider accountProvider;
        private readonly IShareProvider shareProvider;
        public SettingsViewModel(ISectionManager manager, IAccountProvider accProvider, IShareProvider shProvider)
        {
            this.sectionManager = manager;
            this.accountProvider = accProvider;
            this.shareProvider = shProvider;

            this.ShareCommand = new Command(ShareCommand_Execute);
            this.RateReview = new RateRaviewViewModel();
            this.SendFeedbackCommand = new Command(SendFeedbackCommand_Execute);
            this.ShowRateControlCommand = new Command(ShowRateControlCommand_Execute);
            var task = Refresh();
        }
        protected override string ViewTemplateKey => "ViewTemplate";
        protected override string ViewKey => "Settings";
        public Command ShareCommand { get; private set; }
        public Command SendFeedbackCommand { get; private set; }
        public Command ShowRateControlCommand { get; private set; }
        public RateRaviewViewModel RateReview { get; private set; }
        public List<string> Themes { get; private set; } = Enum.GetValues(typeof(Theme)).Cast<Theme>().Select(item => item.ToString()).ToList();
        public string SelectedTheme
        {
            get => accountProvider.DefaultTheme;
            set
            {
                accountProvider.DefaultTheme = value;
            }
        }
        public string AppInfo => $"{ Package.Current.DisplayName}{ Environment.NewLine}{ Environment.NewLine}Developed by Leila Domashenko{ Environment.NewLine}Mentor: Alex Abu Meizer{ Environment.NewLine}{Environment.NewLine}CAD 2020";
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
        #region Command Handlers
        private async void SendFeedbackCommand_Execute()
        {
            await SendFeedback();
        }
        private void ShareCommand_Execute()
        {
            shareProvider.Share();
        }
        private void ShowRateControlCommand_Execute()
        {
            if (this.IsConnected)
            {
                this.RateReview.IsVisible = true;
            }
        }
        #endregion
    }
}
