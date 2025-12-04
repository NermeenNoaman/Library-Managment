using HRSystem.BaseLibrary.Models;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Contracts
{
    public interface IUserRepository : IGenericRepository<USER>
    {
        Task<USER?> GetByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
    }
}

