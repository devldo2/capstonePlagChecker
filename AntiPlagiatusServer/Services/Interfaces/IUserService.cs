using AntiPlagiatusServer.Data.Entities;
using AntiPlagiatusServer.Models;
using AntiPlagiatusServer.Models.DTO;
using System.Collections;
using System.Collections.Generic;

namespace AntiPlagiatusServer.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        string AddUser(UserModel user);
        void DeleteUser(string token);
        string GetToken(string login, string password);
        UserModel GetUser(string token);
        void UpdateTheme(string token, string theme);
        bool IsExistByLogin(string login);
    }
}
