using Microsoft.EntityFrameworkCore;

namespace AntiPlagiatusServer.Data
{
    public interface IDbContext
    {
        DbContext Database { get; }
    }
}
