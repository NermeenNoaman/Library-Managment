// HRSystem.Infrastructure.Implementations.Services/FineService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class FineService : IFineService
{
    private readonly HRSystemContext _context;

    public FineService(HRSystemContext context)
    {
        _context = context;
    }

    // =======================================================
    // 1. Get Fines (للعضو/الجميع)
    // =======================================================
    public async Task<IEnumerable<FINE>> GetMemberFinesAsync(int memberId, bool includePaid = false)
    {
        var query = _context.FINEs.Where(f => f.member_id == memberId);

        if (!includePaid)
        {
            // جلب الغرامات غير المدفوعة فقط افتراضياً
            query = query.Where(f => f.payment_status == "Unpaid"); 
        }

        return await query.ToListAsync();
    }

    // =======================================================
    // 2. Pay Fine (منطق الدفع)
    // =======================================================
    public async Task<FINE> PayFineAsync(int fineId, decimal paymentAmount)
    {
        var fine = await _context.FINEs.FindAsync(fineId);

        if (fine == null)
            throw new Exception("Fine not found.");

        if (fine.payment_status == "Paid")
            throw new Exception("This fine has already been paid.");
            
        // 🛑 التحقق من المبلغ المدفوع
        if (paymentAmount < fine.fine_amount)
            throw new Exception($"Payment amount must be at least {fine.fine_amount:C}.");

        fine.payment_status = "Paid";
        fine.payment_date = DateTime.UtcNow;
        
        _context.FINEs.Update(fine);
        await _context.SaveChangesAsync();

        return fine;
    }
}