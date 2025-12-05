using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly HRSystemContext _context;
    // متغير خاص لتخزين نسخة من الـ CategoryRepository
    private ICategoryRepository? _categories;

    public UnitOfWork(HRSystemContext context)
    {
        _context = context;
    }

    // خاصية للوصول إلى Category Repository
    public ICategoryRepository Categories
    {
        get { return _categories ??= new CategoryRepository(_context); }
    }

    // دالة حفظ التغييرات
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // دالة التخلص من الموارد
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}