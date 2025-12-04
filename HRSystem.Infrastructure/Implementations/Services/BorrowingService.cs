using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingRepository _repo;
        private readonly HRSystemContext _context;
        private readonly IMapper _mapper;

        private const int BorrowLimit = 3;
        private const decimal DailyFineAmount = 50m;

        public BorrowingService(IBorrowingRepository repo, HRSystemContext context, IMapper mapper)
        {
            _repo = repo;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BorrowingReadDto> BorrowBookAsync(BorrowingCreateDto dto)
        {
            if (await _repo.HasUnpaidFinesAsync(dto.MemberId))
                throw new Exception("Cannot borrow: Member has unpaid fines.");

            var activeCount = await _repo.CountActiveBorrowingsAsync(dto.MemberId);
            if (activeCount >= BorrowLimit)
                throw new Exception($"Cannot borrow: Member reached the limit of {BorrowLimit} active borrowings.");

            var book = await _context.BOOKs.FindAsync(dto.BookId);
            if (book == null || book.available_copies <= 0)
                throw new Exception("Cannot borrow: Book is currently unavailable.");

            var borrowing = new BORROWING
            {
                member_id = dto.MemberId,
                book_id = dto.BookId,
                borrow_date = DateTime.UtcNow,
                due_date = DateTime.UtcNow.AddDays(7),
                status = "Borrowed"
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _repo.AddAsync(borrowing);
                book.available_copies--;
                _context.BOOKs.Update(book);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<BorrowingReadDto>(borrowing);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<BorrowingReadDto> ReturnBookAsync(int borrowingId)
        {
            var borrowing = await _repo.GetBorrowingWithBookAsync(borrowingId);
            if (borrowing == null)
                throw new Exception("Borrowing not found.");

            if (borrowing.status == "Returned")
                throw new Exception("This book has already been returned.");

            var now = DateTime.UtcNow;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // حساب الغرامة إذا متأخر
                if (now > borrowing.due_date)
                {
                    var daysLate = (int)Math.Ceiling((now - borrowing.due_date).TotalDays);
                    var fine = new FINE
                    {
                        member_id = borrowing.member_id,
                        borrowing_id = borrowing.borrowing_id,
                        fine_amount = daysLate * DailyFineAmount,
                        fine_date = now,
                        payment_status = "Unpaid"
                    };
                    await _context.FINEs.AddAsync(fine);
                }

                borrowing.return_date = now;
                borrowing.status = "Returned";
                borrowing.book.available_copies++;
                _context.BOOKs.Update(borrowing.book);
                _context.BORROWINGs.Update(borrowing);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<BorrowingReadDto>(borrowing);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
