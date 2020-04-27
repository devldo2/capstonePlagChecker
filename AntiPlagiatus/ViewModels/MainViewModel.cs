using AntiPlagiatus.Helpers;
using AntiPlagiatus.Models;
using AntiPlagiatus.Models.Inferfaces;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Providers.Interfaces;
using AntiPlagiatus.Views.Resources;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AntiPlagiatus.ViewModels
{
    public class MainViewModel : BaseViewModel, ISectionManager
    {
        private readonly IAccountProvider accountProvider;
        private readonly IShareProvider shareProvider;
        private readonly IHistoryProvider historyProvider;
        private readonly CurrentCheckProvider currentCheckProvider = new CurrentCheckProvider();

        private BaseViewModel currentSectionUI;
        private SectionType sectionType = SectionType.Check;
        public MainViewModel(IAccountProvider acService, IShareProvider shProvider, IHistoryProvider hiProvider)
        {
            this.accountProvider = acService;
            this.shareProvider = shProvider;
            this.historyProvider = hiProvider;

            this.ChangeSection = new Command<string>(ChangeSectionExecute);
            this.ChangeSectionExecute(this.sectionType.ToString());
            accountProvider.AccountUpdated += AccountProvider_AccountUpdated;
            accountProvider.DefaultThemeUpdated += Instance_DefaultThemeUpdated;
        }
        protected override string ViewTemplateKey => "ViewTemplate";
        protected override string ViewKey => "Main";
        public Command<string> ChangeSection { get; set; }
        public BaseViewModel CurrentSection
        {
            get => this.currentSectionUI;
            set
            {
                if (this.currentSectionUI != value)
                {
                    if (this.currentSectionUI != null)
                        this.currentSectionUI.Dispose();
                    this.currentSectionUI = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public SectionType SectionType
        {
            get => sectionType;
            set
            {
                if (sectionType != value)
                {
                    this.sectionType = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private void AccountProvider_AccountUpdated(object sender, string e)
        {
            historyProvider.Refresh();
        }
        private void Instance_DefaultThemeUpdated(object sender, string theme)
        {
            ResourceDictionary resourceDict = ResourceHelper.GetMainTheme(theme);
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, resourceDict);
            var mainPage = Window.Current.Content as MainPage;
            if (mainPage != null)
            { var task = this.Refresh(); }
        }
        protected async override Task<Exception> OnRefresh(object parameter = null)
        {
            Exception exception = null;
            try
            {
                if (this.CurrentSection != null)
                {
                    Task task = this.CurrentSection.Refresh();
                }
                this.ViewTemplate = ResourceHelper.GetViewModelTemplate(this.ViewKey, ViewTemplateKey);
            }
            catch (Exception ex)
            {
                //exception not with api but with request
                exception = ex;
            }
            return exception;
        }
        public override void Dispose()
        {
            base.Dispose();
            accountProvider.DefaultThemeUpdated -= Instance_DefaultThemeUpdated;
            accountProvider.AccountUpdated -= AccountProvider_AccountUpdated;
        }
        private void ChangeSectionExecute(string sectionName)
        {
            SectionType selectedSection;
            if (Enum.TryParse(sectionName, out selectedSection))
            {
                this.SectionType = selectedSection;

                switch (this.SectionType)
                {
                    case SectionType.Check:
                        this.CurrentSection = new CheckViewModel(this, this.historyProvider, this.currentCheckProvider);
                        break;
                    case SectionType.History:
                        this.CurrentSection = new HistoryViewModel(this, historyProvider, currentCheckProvider);
                        break;
                    case SectionType.Settings:
                        this.CurrentSection = new SettingsViewModel(this, accountProvider, shareProvider);
                        break;
                    case SectionType.Account:
                        this.CurrentSection = new LoginViewModel(this, accountProvider);
                        break;
                }
                var task = Refresh();
            }
        }
        public void GoTo(SectionType sectionType)
        {
            this.ChangeSectionExecute(sectionType.ToString());
        }
    }
}
