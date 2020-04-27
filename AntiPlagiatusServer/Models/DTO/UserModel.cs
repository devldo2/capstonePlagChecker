namespace AntiPlagiatusServer.Models.DTO
{
    public class UserModel
    {
        public UserModel()
        {
        }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string DefaultTheme { get; set; }
    }
}
