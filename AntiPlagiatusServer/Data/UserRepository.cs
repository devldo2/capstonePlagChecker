using AntiPlagiatusServer.Data.Entities;

namespace AntiPlagiatusServer.Data
{
    public class UserRepository : IUserRepository
    {
        private IDbContext dbContext;
        private IRepository<User> userRepository;
        public UserRepository(IDbContext context)
        {
            dbContext = context;
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new Repository<User>(dbContext);
                return userRepository;
            }
        }
    }
}
