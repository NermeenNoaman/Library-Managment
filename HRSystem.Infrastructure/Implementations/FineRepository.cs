// HRSystem.Infrastructure.Implementations/FineRepository.cs

using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

// يجب أن يتبع هذا الهيكل الذي تستخدمينه (قد يختلف حسب الـ GenericRepository)
public class FineRepository : GenericRepository<FINE>, IFineRepository
{
    private readonly HRSystemContext _context;
    public FineRepository(HRSystemContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FINE>> GetUnpaidFinesByMemberIdAsync(int memberId)
    {
        // 🚨 تصحيح: نستخدم PaymentStatus.Unpaid كـ Enum
        return await _context.FINEs
            .Where(f => f.member_id == memberId && f.payment_status == "Unpaid")
            .ToListAsync();
    }
    
    public async Task<FINE> GetFineByIdAsync(int fineId)
    {
        return await _context.FINEs.FindAsync(fineId);
    }
}