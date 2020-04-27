using System;
using System.Threading.Tasks;

namespace AntiPlagiatus.Providers
{
    public interface IAccountProvider
    {
        event EventHandler<string> AccountUpdated;
        event EventHandler<string> DefaultThemeUpdated;
        string DefaultTheme { get; set; }
        bool IsLogged { get; }
        string GetToken();
        Task Register(string email, string password);
        Task Login(string email, string password);
        void Logout();
        Task UpdateTheme();
        Task Initialize();
    }
}
