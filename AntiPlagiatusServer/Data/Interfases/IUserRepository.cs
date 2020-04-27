using AntiPlagiatusServer.Data.Entities;

namespace AntiPlagiatusServer.Data
{
    //TODO
    public interface IUserRepository
    {
        IRepository<User> Users { get; }
    }
}
