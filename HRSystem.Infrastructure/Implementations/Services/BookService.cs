using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Infrastructure.Implementations.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;

        public BookService(IBookRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        // =======================================================
        // 1. Create Book
        // =======================================================
        public async Task<BookReadDto> CreateBookAsync(BookCreateDto dto)
        {
            var isUnique = await _repo.IsIsbnUniqueAsync(dto.Isbn);

            if (!isUnique)
            {
                throw new ArgumentException($"Book with ISBN '{dto.Isbn}' already exists.");
            }


            // 2. التحويل من DTO إلى Entity
            var bookEntity = _mapper.Map<BOOK>(dto);

            // 3. تعيين الحقول الافتراضية ومنطق العمل
            bookEntity.available_copies = bookEntity.total_copies; // المتاح = الكلي عند الإنشاء
            bookEntity.status = "Available"; // تعيين الحالة الافتراضية
            bookEntity.created_at = DateTime.UtcNow;

            // 4. الحفظ
            await _repo.AddAsync(bookEntity);
            await _repo.SaveChangesAsync();

            // 5. جلب الكيان بالتفاصيل (Category) لإرجاع ReadDto كامل
            var createdBook = await _repo.GetBookWithDetailsByIdAsync(bookEntity.book_id);

            return _mapper.Map<BookReadDto>(createdBook);
        }

        // =======================================================
        // 2. Get All Books
        // =======================================================
        public async Task<IEnumerable<BookReadDto>> GetAllBooksAsync()
        {
            // نستخدم دالة GetAllAsync() العادية (إذا لم تتجاوزها، ستحتاج الـ Controller لربط الـ Category)
            // إذا قمت بتجاوز GetAllAsync في Repository لتضمين Category، فالأمر صحيح.
            var entities = await _repo.GetAllAsync();

            // Note: If GetAllAsync doesn't include Category, CategoryName in DTO will be null.
            return _mapper.Map<IEnumerable<BookReadDto>>(entities);
        }

        // =======================================================
        // 3. Get Book by ID
        // =======================================================
        public async Task<BookReadDto> GetBookByIdAsync(int id)
        {
            // نستخدم دالة GetBookWithDetailsByIdAsync لضمان جلب اسم التصنيف
            var entity = await _repo.GetBookWithDetailsByIdAsync(id);
            if (entity == null) return null;

            return _mapper.Map<BookReadDto>(entity);
        }

        // =======================================================
        // 4. Search Books by Title
        // =======================================================
        public async Task<IEnumerable<BookReadDto>> SearchBooksByTitleAsync(string titleKeyword)
        {
            var entities = await _repo.GetBooksByTitleAsync(titleKeyword);
            return _mapper.Map<IEnumerable<BookReadDto>>(entities);
        }

        // =======================================================
        // 5. Get Books by Category ID (للتصفية)
        // =======================================================
        public async Task<IEnumerable<BookReadDto>> GetBooksByCategoryIdAsync(int categoryId)
        {
            var entities = await _repo.GetBooksByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<BookReadDto>>(entities);
        }

        // =======================================================
        // 6. Update Book
        // =======================================================
        public async Task<bool> UpdateBookAsync(int id, BookUpdateDto dto)
        {
            var existingBook = await _repo.GetByIdAsync(id);
            if (existingBook == null) return false;

            // إذا تم تغيير TotalCopies، يجب تحديث AvailableCopies
            int copiesDifference = 0;
            if (dto.TotalCopies.HasValue && dto.TotalCopies.Value != existingBook.total_copies)
            {
                copiesDifference = dto.TotalCopies.Value - existingBook.total_copies;
            }

            // 1. تطبيق التحديثات من DTO (AutoMapper يطبق فقط الحقول غير Null)
            _mapper.Map(dto, existingBook);

            // 2. تحديث AvailableCopies بناءً على الفرق
            if (copiesDifference != 0)
            {
                existingBook.available_copies += copiesDifference;
                // التأكد من أن available_copies لا تصبح سالبة
                if (existingBook.available_copies < 0) existingBook.available_copies = 0;
            }

            // 3. تعيين تاريخ التحديث
            existingBook.updated_at = DateTime.UtcNow;

            await _repo.UpdateAsync(existingBook);
            await _repo.SaveChangesAsync();
            return true;
        }

        // =======================================================
        // 7. Delete Book
        // =======================================================
        public async Task<bool> DeleteBookAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            // ⚠️ ملاحظة: يجب التحقق هنا مما إذا كان الكتاب مُعارًا (BORROWINGs) قبل حذفه.

            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}