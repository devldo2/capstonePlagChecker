using AntiPlagiatus.Models;
using AntiPlagiatus.Models.API;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace AntiPlagiatus.Providers
{
    public class LocalAcountProvider : IAccountProvider
    {
        private string defaultTheme = Theme.Dark.ToString();
        private string token;
        private bool isInitialized;

        public event EventHandler<string> AccountUpdated;
        public event EventHandler<string> DefaultThemeUpdated;
        public string DefaultTheme
        {
            get => defaultTheme;
            set
            {
                if (this.defaultTheme != value)
                {
                    defaultTheme = value;
                    Task task = UpdateTheme();
                }
            }
        }
        public bool IsLogged => isInitialized && !string.IsNullOrEmpty(token);
        public async Task Register(string email, string password)
        {
            var apiresult = await WebApiProvider.RegisterUser(email, password);
            if (!string.IsNullOrEmpty(apiresult.ErrorMessage))
                throw new Exception(apiresult.ErrorMessage);
            else
            {
                var user = apiresult.Content as APIUser;
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.Token))
                    {
                        this.token = user.Token;
                        SettingsProvider.WriteValueIntoAppData<string>(Constants.UserIDKey, this.token);
                        this.AccountUpdated?.Invoke(this, this.token);
                    }
                }
            }
        }
        public async Task Login(string email, string password)
        {
            var apiresult = await WebApiProvider.LoginUser(email, password);
            if (!string.IsNullOrEmpty(apiresult.ErrorMessage))
                throw new Exception(apiresult.ErrorMessage);
            else
            {
                var user = apiresult.Content as APIUser;
                if (user != null && !string.IsNullOrEmpty(user.Token))
                {
                    //check it
                    if (this.defaultTheme != user.DefaultTheme)
                    {

                        this.defaultTheme = user.DefaultTheme;
                        this.DefaultThemeUpdated?.Invoke(this, this.defaultTheme);
                    }
                    this.token = user.Token;
                    SettingsProvider.WriteValueIntoAppData<string>(Constants.UserIDKey, this.token);
                    this.AccountUpdated?.Invoke(this, this.token);
                }
            }
        }
        public void Logout()
        {
            this.token = null;
            SettingsProvider.WriteValueIntoAppData<string>(Constants.UserIDKey, this.token);
            this.AccountUpdated?.Invoke(this, this.token);
        }
        public async Task UpdateTheme()
        {
            this.DefaultThemeUpdated?.Invoke(this, this.defaultTheme);
            if (IsLogged)
            {
                var apiresult = await WebApiProvider.UpdateTheme(token, defaultTheme);
                if (!string.IsNullOrEmpty(apiresult.ErrorMessage))
                    throw new Exception(apiresult.ErrorMessage);
            }
        }
        public string GetToken() => this.token;
        public async Task Initialize()
        {
            var userToken = SettingsProvider.ReadValueFromAppData<string>(Constants.UserIDKey);
            if (!string.IsNullOrEmpty(userToken))
            {
                var apiresult = await WebApiProvider.IsUserExist(userToken);
                if (apiresult.Content is APIUser && string.IsNullOrEmpty(apiresult.ErrorMessage))
                {
                    token = userToken;
                    if (this.defaultTheme != ((APIUser)apiresult.Content).DefaultTheme)
                    {

                        this.defaultTheme = ((APIUser)apiresult.Content).DefaultTheme;
                        this.DefaultThemeUpdated?.Invoke(this, this.defaultTheme);
                    }
                }
                else
                {
                    MessageDialog dialog = new MessageDialog(apiresult.ErrorMessage, "Server Error");
                    await dialog.ShowAsync();
                }
            }
            else
                SettingsProvider.WriteValueIntoAppData<string>(Constants.UserIDKey, this.token);
            isInitialized = true;
            this.AccountUpdated?.Invoke(this, this.token);
        }
    }
}
