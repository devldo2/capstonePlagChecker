using AntiPlagiatus.Helpers;
using AntiPlagiatus.Helpers.Extensions;
using AntiPlagiatus.Models.Inferfaces;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Views.Resources;
using System;
using System.Threading.Tasks;

namespace AntiPlagiatus.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private readonly ISectionManager sectionManager;
        private readonly IAccountProvider accountProvider;

        private string email;
        private string password;
        private string confirmedPassword;
        public LoginViewModel(ISectionManager manager, IAccountProvider accProvider)
        {
            this.sectionManager = manager;
            this.accountProvider = accProvider;

            this.RegisterCommand = new Command(RegisterCommand_Execute);
            this.LoginLogoutCommand = new Command(LoginLogoutCommand_Execute);
            this.SwitchToRegisterCommand = new Command(SwitchToRegisterCommand_Execute);
            var task = Refresh();
        }
        protected override string ViewTemplateKey => "ViewTemplate";
        protected override string ViewKey => "Login";
        public string MessageError { get; private set; }
        public bool IsRegistration { get; private set; }
        public bool IsLogged => accountProvider.IsLogged;
        public string Email
        {
            get => email;
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string Password
        {
            get => password;
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public string ConfirmedPassword
        {
            get => confirmedPassword;
            set
            {
                if (this.confirmedPassword != value)
                {
                    this.confirmedPassword = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public Command LoginLogoutCommand { get; set; }
        public Command RegisterCommand { get; set; }
        public Command SwitchToRegisterCommand { get; set; }
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
        #region CommandHandlers
        private async void RegisterCommand_Execute()
        {
            this.MessageError = string.Empty;
            if (string.IsNullOrEmpty(email)) this.MessageError += $"Login field is empty{Environment.NewLine}";
            else if(!email.IsValidEmail()) this.MessageError += $"Login must be an email{Environment.NewLine}";
            if (string.IsNullOrEmpty(password)) this.MessageError += $"Password field is empty{Environment.NewLine}";
            else if (password != confirmedPassword) this.MessageError += $"The password and confirmed password don't match{Environment.NewLine}";
            //This email is already in use.
            if (string.IsNullOrEmpty(this.MessageError))
            {
                try
                {
                    await accountProvider.Register(this.email, this.password);
                    this.RaisePropertyChanged(() => this.IsLogged);
                }
                catch(Exception ex)
                {
                    this.MessageError = ex.Message;
                }
               
            }
            //if success clear fiels, otherwise show an error
            this.RaisePropertyChanged(() => this.MessageError);
        }
        private async void LoginLogoutCommand_Execute()
        {
            this.MessageError = string.Empty;
            if (this.IsLogged)
                accountProvider.Logout();
            else
            {
                if (string.IsNullOrEmpty(email)) this.MessageError += $"Login field is empty{Environment.NewLine}";
                else if (!email.IsValidEmail()) this.MessageError += $"Login must be an email{Environment.NewLine}";
                if (string.IsNullOrEmpty(password)) this.MessageError += $"Password field is empty{Environment.NewLine}";

                if (string.IsNullOrEmpty(MessageError))
                    try
                    {
                        await accountProvider.Login(this.email, this.password);
                        //if success clear fiels, otherwise show an error
                    }
                    catch(Exception ex)
                    {
                        this.MessageError = ex.Message;
                    }
            }

            this.RaisePropertyChanged(() => this.MessageError);
            this.RaisePropertyChanged(() => this.IsLogged);
        }
        private void SwitchToRegisterCommand_Execute()
        {
            this.MessageError = string.Empty;
            this.RaisePropertyChanged(() => this.MessageError);

            this.Email = string.Empty;
            this.Password = string.Empty;
            this.ConfirmedPassword = string.Empty;

            this.IsRegistration = !this.IsRegistration;
            this.RaisePropertyChanged(() => this.IsRegistration);
        }
        #endregion
    }
}
