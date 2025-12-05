using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Implementations;

public class CategoryRepository : GenericRepository<CATEGORY>, ICategoryRepository
{
    public CategoryRepository(HRSystemContext context) : base(context)
    {
    }

    public async Task<CATEGORY?> GetByNameAsync(string name)
    {
        return await _context.CATEGORies.FirstOrDefaultAsync(c => c.category_name == name);
    }
}