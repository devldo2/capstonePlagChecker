using AntiPlagiatusServer.Models;
using AntiPlagiatusServer.Models.DTO;
using AntiPlagiatusServer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AntiPlagiatusServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        [Route("GetAll")]
        private async Task<ObjectResult> GetAll()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonContent);
                    var users = this.userService.GetAll();
                    return new OkObjectResult(users);
                }
            }
            catch (Exception ex) { return StatusCode(404, ex.InnerException?.GetBaseException().Message); }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ObjectResult> Register()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonContent);
                    var token = this.userService.AddUser(userModel);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var user = userService.GetUser(token);
                        return new OkObjectResult(user);
                    }
                    else return StatusCode(500, "Token is invalid. Please connect with admin");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.GetBaseException().Message);
            }
        }
        [HttpGet]
        [Route("Login")]
        public async Task<ObjectResult> Login()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonContent);
                    string token = this.userService.GetToken(userModel.Login, userModel.Password);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var user = userService.GetUser(token);
                        return new OkObjectResult(user);
                    }
                    else return StatusCode(400, "Token is invalid");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.GetBaseException().Message);
            }
        }
        [HttpGet]
        [Route("CheckUser")]
        public async Task<ObjectResult> CheckUser()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonContent);
                    UserModel user = this.userService.GetUser(userModel.Token);
                    return new OkObjectResult(user);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.GetBaseException().Message);
            }
        }
        [HttpPut]
        [Route("UpdateTheme")]
        public async Task<IActionResult> UpdateTheme()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonContent);
                    this.userService.UpdateTheme(userModel.Token, userModel.DefaultTheme);
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
