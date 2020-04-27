using AntiPlagiatusServer.Data;
using AntiPlagiatusServer.Data.Entities;
using AntiPlagiatusServer.Models.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AntiPlagiatusServer.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private IMapper mapper;
        public UserService(IUserRepository userRep, IMapper mapp)
        {
            this.userRepository = userRep;
            this.mapper = mapp;
        }
        public string AddUser(UserModel userModel)
        {
            if (userModel != null)
            {
                try
                {
                    var user = mapper.Map<UserModel, User>(userModel);
                    if (this.userRepository.Users.GetAll().FirstOrDefault(item => item.Login == userModel.Login.ToLower()) == null)
                    {
                        user.Token = Guid.NewGuid().ToString();
                        user.Login = user.Login.ToLower();
                        this.userRepository.Users.Add(user);
                        this.userRepository.Users.Save();
                        return user.Token;
                    }
                    else throw new InvalidOperationException("User has already exist in database");
                }
                catch (Exception ex)
                {
                    throw new Exception($"AddUser: {ex.Message}", ex.InnerException ?? ex);
                }
            }

            return null;
        }
        public void UpdateTheme(string token, string theme)
        {
            try
            {
                var user = this.userRepository.Users.GetAll().FirstOrDefault(item => item.Token == token);
                if (user == null)
                    throw new FileNotFoundException("User doesn't exist in database.");
                else
                {
                    user.DefaultTheme = theme;
                    userRepository.Users.Edit(user);
                    userRepository.Users.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"GetToken: {ex.Message}", ex.InnerException ?? ex);
            }
        }
        public void DeleteUser(string token)
        {
            try
            {
                var user = this.userRepository.Users.GetAll().FirstOrDefault(item => item.Token == token);
                if (user != null)
                {
                    this.userRepository.Users.Delete(user);
                    this.userRepository.Users.Save();
                }
                else throw new FileNotFoundException("User doesn't exist in database.");
            }

            catch (Exception ex)
            {
                throw new Exception($"DeleteUser: {ex.Message}", ex.InnerException ?? ex);
            }
        }

        public UserModel GetUser(string token)
        {
            try
            {
                var user = this.userRepository.Users.GetAll().FirstOrDefault(item => item.Token == token);
                if (user == null)
                    throw new FileNotFoundException("User doesn't exist in database.");
                else return mapper.Map<User, UserModel>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"GetUser: {ex.Message}", ex.InnerException ?? ex);
            }
        }

        public bool IsExistByLogin(string email)
        {
            try
            {
                return this.userRepository.Users.GetAll().FirstOrDefault(item => item.Login == email.ToLower()) != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"IsExist: {ex.Message}", ex.InnerException ?? ex);
            }
        }

        public string GetToken(string email, string password)
        {
            try
            {
                var user = this.userRepository.Users.GetAll().FirstOrDefault(item => item.Login == email.ToLower() && item.Password == password);
                if (user == null)
                    throw new FileNotFoundException("User doesn't exist in database.");
                else return user.Token;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetToken: {ex.Message}", ex.InnerException ?? ex);
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            try
            {
                return mapper.Map<IEnumerable<UserModel>>(userRepository.Users.GetAll().ToList());
            }
            catch (Exception ex)
            {
                throw new Exception($"GetToken: {ex.Message}", ex.InnerException ?? ex);
            }
        }
    }
}
