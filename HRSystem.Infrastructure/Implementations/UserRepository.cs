using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Infrastructure.Implementations
{
    public class UserRepository : GenericRepository<USER>, IUserRepository
    {
        public UserRepository(HRSystemContext context) : base(context)
        {
        }

        public async Task<USER?> GetByUsernameAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.email == email);
        }


        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(x => x.email == email);
        }

        public override async Task<IEnumerable<USER>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.MEMBER).ToListAsync();
        }
    }
}

