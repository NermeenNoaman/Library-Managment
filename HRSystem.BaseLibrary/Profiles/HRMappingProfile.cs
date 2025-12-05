// HR Mapping Profile for Entities and DTOs

using AutoMapper;
// Import all DTOs from the DTOs folder
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace HRSystem.BaseLibrary.Profiles // Using the specified Profiles namespace
{
    // This class inherits from AutoMapper's Profile
    public class HRMappingProfile : Profile
    {
        public HRMappingProfile()
        {
            // =========================================================================
            // 1. DTOs for USER 
            // =========================================================================

            // 1. Read: Convert Entity (DB: USER) to ReadDto (Output: UserReadDto)
            CreateMap<USER, UserReadDto>()
                .ForMember(dest => dest.user_id, opt => opt.MapFrom(src => src.user_id))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role))
                .ForMember(dest => dest.fullname, opt => opt.MapFrom(src => src.fullname))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.phone));

            // 2. Register: Convert RegisterDto (Input) to Entity (for adding new record)
            CreateMap<UserRegisterDto, USER>() 
                .ForMember(dest => dest.user_id, opt => opt.Ignore())
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.Role));


            // =========================================================================
            // 2. DTOs for LIBRARY 
            // =========================================================================
            CreateMap<LIBRARY, LibraryReadDto>()
            .ForMember(dest => dest.LibraryId, opt => opt.MapFrom(src => src.library_id))
            .ForMember(dest => dest.LibraryName, opt => opt.MapFrom(src => src.library_name))
            .ForMember(dest => dest.LibraryEmail, opt => opt.MapFrom(src => src.library_email))
            .ForMember(dest => dest.LibraryPhone, opt => opt.MapFrom(src => src.library_phone))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.created_at));

            // Create Mapping
            CreateMap<LibraryCreateDto, LIBRARY>()
                .ForMember(dest => dest.library_id, opt => opt.Ignore())

                .ForMember(dest => dest.library_name, opt => opt.MapFrom(src => src.LibraryName))
                .ForMember(dest => dest.tax_number, opt => opt.MapFrom(src => src.TaxNumber))
                .ForMember(dest => dest.library_email, opt => opt.MapFrom(src => src.LibraryEmail))
                .ForMember(dest => dest.library_phone, opt => opt.MapFrom(src => src.LibraryPhone))
                .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => DateTime.Now));

            // Update Mapping
            CreateMap<LibraryUpdateDto, LIBRARY>()
                 .ForMember(dest => dest.library_name, opt => opt.MapFrom(src => src.LibraryName))
                .ForMember(dest => dest.tax_number, opt => opt.MapFrom(src => src.TaxNumber))
                .ForMember(dest => dest.library_email, opt => opt.MapFrom(src => src.LibraryEmail))
                .ForMember(dest => dest.library_phone, opt => opt.MapFrom(src => src.LibraryPhone))
                .ForMember(dest => dest.updated_at, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));



            // =========================================================================
            // 3. DTOs for LIBRARY_BRANCH (Library Branches Management)
            // =========================================================================

            // 1. Read Mapping: Entity to ReadDto (Output)
            CreateMap<LIBRARY_BRANCH, LibraryBranchReadDto>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.branch_id))
                .ForMember(dest => dest.LibraryId, opt => opt.MapFrom(src => src.library_id))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.branch_name))
                .ForMember(dest => dest.BranchLocation, opt => opt.MapFrom(src => src.branch_location))
                .ForMember(dest => dest.BranchPhone, opt => opt.MapFrom(src => src.branch_phone))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.created_at));

            // 2. Create Mapping: DTO to Entity (Input)
            CreateMap<LibraryBranchCreateDto, LIBRARY_BRANCH>()
                .ForMember(dest => dest.branch_id, opt => opt.Ignore()) // PK is Auto-Generated
                .ForMember(dest => dest.library_id, opt => opt.MapFrom(src => src.LibraryId))
                .ForMember(dest => dest.branch_name, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.branch_location, opt => opt.MapFrom(src => src.BranchLocation))
                .ForMember(dest => dest.branch_phone, opt => opt.MapFrom(src => src.BranchPhone))  
                .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => DateTime.Now)); // Set creation date

            // 3. Update Mapping: DTO to Entity (Allows partial updates)
            CreateMap<LibraryBranchUpdateDto, LIBRARY_BRANCH>()
                .ForMember(dest => dest.updated_at, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.branch_phone, opt => opt.MapFrom(src => src.BranchPhone))
                .ForMember(dest => dest.library_id, opt => opt.MapFrom(src => src.LibraryId))
                .ForMember(dest => dest.branch_name, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.branch_location, opt => opt.MapFrom(src => src.BranchLocation))

                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null source values



            // =========================================================================
            // 4. DTOs for CATEGORY
            // =========================================================================
            // 1. Read Mapping: Entity to ReadDto (Output)
            CreateMap<CATEGORY, CategoryReadDto>()
                 .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.category_id))
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category_name))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description));

            // 2. Create/Update Mapping: DTO to Entity (Input)
            CreateMap<CategoryCreateUpdateDto, CATEGORY>()
                 .ForMember(dest => dest.category_id, opt => opt.Ignore()) // PK is Auto-Generated/Ignored on input
                 .ForMember(dest => dest.category_name, opt => opt.MapFrom(src => src.CategoryName))
                 .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description));

            
            // =========================================================================
            // 5. DTOs for MEMBER (Member Management) - NEW MAPPING
            // =========================================================================

            // 1. Read Mapping: Entity (MEMBER) to ReadDto (Output)
            CreateMap<MEMBER, MemberReadDto>()
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.member_id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.user_id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.first_name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.last_name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.date_of_birth))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.registration_date))
                .ForMember(dest => dest.MembershipType, opt => opt.MapFrom(src => src.membership_type))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status));

            // 2. Create Mapping: DTO (MemberCreateDto) to Entity (Input)
            CreateMap<MemberCreateDto, MEMBER>()
                .ForMember(dest => dest.member_id, opt => opt.Ignore()) // PK handled by DB
                .ForMember(dest => dest.user_id, opt => opt.Ignore())   // FK set by Repository after User creation
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.date_of_birth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.membership_type, opt => opt.MapFrom(src => src.MembershipType))
                // Dates and Status are handled in the Repository/Service logic (CreateMemberWithUserAsync)
                .ForMember(dest => dest.registration_date, opt => opt.Ignore())
                .ForMember(dest => dest.status, opt => opt.Ignore());

            // 3. Update Mapping: DTO (MemberUpdateDto) to Entity (Input)
            CreateMap<MemberUpdateDto, MEMBER>()
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.date_of_birth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.membership_type, opt => opt.MapFrom(src => src.MembershipType))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.updated_at, opt => opt.Ignore()) 
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));




            // =========================================================================
            // 6. DTOs for Librarian 
            // =========================================================================
            //1. Read Mapping: Entity to ReadDto (Output)
            CreateMap<LIBRARIAN, LibrarianReadDto>()
                .ForMember(dest => dest.LibrarianId, opt => opt.MapFrom(src => src.librarian_id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.first_name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.last_name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.salary))
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.hire_date))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status));
            //2. Create Mapping: DTO to Entity (Input)
            CreateMap<LibrarianCreateDto, LIBRARIAN>()
                .ForMember(dest => dest.librarian_id, opt => opt.Ignore()) // PK is Auto-Generated
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.hire_date, opt => opt.MapFrom(src => src.HireDate))
                .ForMember(dest => dest.status, opt => opt.Ignore())       // Set in Repository/Service
                .ForMember(dest => dest.created_at, opt => opt.Ignore()) // Set in Repository/Service
                .ForMember(dest => dest.branch_id, opt => opt.MapFrom(src => src.BranchId));
            //3. Update Mapping: DTO to Entity (Input)
            CreateMap<LibrarianUpdateDto, LIBRARIAN>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.updated_at, opt => opt.Ignore()) // Set in Repository/Service
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null source values







            // =========================================================================
            // 7. DTOs for BOOK 
            // =========================================================================
            // 1. Read Mapping: Entity (BOOK) to ReadDto (Output)
            CreateMap<BOOK, BookReadDto>()
                .ForMember(dest => dest.Isbn, opt => opt.MapFrom(src => src.isbn))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.book_id))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.category_id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.author_name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.publisher))
                .ForMember(dest => dest.PublicationYear, opt => opt.MapFrom(src => src.publication_year))
                .ForMember(dest => dest.TotalCopies, opt => opt.MapFrom(src => src.total_copies))
                .ForMember(dest => dest.AvailableCopies, opt => opt.MapFrom(src => src.available_copies))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.language))
                .ForMember(dest => dest.Pages, opt => opt.MapFrom(src => src.pages))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description));




            // 2. Create Mapping: DTO (BookCreateDto) to Entity (Input)
            CreateMap<BookCreateDto, BOOK>()
                .ForMember(dest => dest.isbn, opt => opt.MapFrom(src => src.Isbn))
                .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.author_name, opt => opt.MapFrom(src => src.AuthorName))
                .ForMember(dest => dest.category_id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.total_copies, opt => opt.MapFrom(src => src.TotalCopies))
                .ForMember(dest => dest.publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(dest => dest.publication_year, opt => opt.MapFrom(src => src.PublicationYear))
                .ForMember(dest => dest.total_copies, opt => opt.MapFrom(src => src.TotalCopies))
                .ForMember(dest => dest.language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.pages, opt => opt.MapFrom(src => src.Pages))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.book_id, opt => opt.Ignore()) // PK handled by DB
                .ForMember(dest => dest.status, opt => opt.Ignore()) // Set by Service
                .ForMember(dest => dest.created_at, opt => opt.Ignore()) // Set by Service
                .ForMember(dest => dest.updated_at, opt => opt.Ignore()) // Not set on create
                .ForMember(dest => dest.available_copies, opt => opt.Ignore()); // Set by Service (يساوي TotalCopies)

            // 3. Update Mapping: DTO (BookUpdateDto) to Entity (Input)
            CreateMap<BookUpdateDto, BOOK>()
                .ForMember(dest => dest.updated_at, opt => opt.Ignore()) // Set by Service
                .ForMember(dest => dest.isbn, opt => opt.Ignore()) 
                .ForMember(dest => dest.available_copies, opt => opt.Ignore()) 
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // =========================================================================
            // 8. DTOs for Borrowing
            // =========================================================================
            // 1. Read Mapping: Entity (BOOK) to ReadDto (Output)
            CreateMap<BORROWING, BorrowingReadDto>()
                .ForMember(dest => dest.BorrowingId, opt => opt.MapFrom(src => src.borrowing_id))
                .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => src.borrow_date))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.due_date))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.return_date))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.member_id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.book_id));


            // 2. Create Mapping: DTO (BookCreateDto) to Entity (Input)
            CreateMap<BORROWING, BorrowingCreateDto>()
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.member_id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.book_id));


            // 3. Update Mapping: DTO (BookUpdateDto) to Entity (Input)
            CreateMap<BORROWING, BorrowingUpdateDto>()
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.return_date))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status));


            // =========================================================================
            // 9. DTOs for FINE (Fines Management) - NEW MAPPING
            // =========================================================================

            // 1. Read Mapping: Entity (FINE) to ReadDto (Output)
            CreateMap<FINE, FineReadDto>()
                .ForMember(dest => dest.FineId, opt => opt.MapFrom(src => src.fine_id))
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.member_id))
                .ForMember(dest => dest.BorrowingId, opt => opt.MapFrom(src => src.borrowing_id))
                .ForMember(dest => dest.FineAmount, opt => opt.MapFrom(src => src.fine_amount))
                .ForMember(dest => dest.FineDate, opt => opt.MapFrom(src => src.fine_date))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.payment_status))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.payment_date));

            // 2. Pay Mapping: DTO (FinePayDto) is handled in Service logic, no direct Entity mapping needed.

            // =========================================================================
            // 10. DTOs for RESERVATION (Reservation Management) - NEW MAPPING
            // =========================================================================

            // 1. Read Mapping: Entity (RESERVATION) to ReadDto (Output)
            CreateMap<RESERVATION, ReservationReadDto>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.reservation_id))
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.member_id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.book_id))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.reservation_date))
                .ForMember(dest => dest.ArriveDate, opt => opt.MapFrom(src => src.arrive_date))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.PriorityNumber, opt => opt.MapFrom(src => src.priority_number));

            // 2. Create Mapping: DTO (ReservationCreateDto) to Entity (Input)
            CreateMap<ReservationCreateDto, RESERVATION>()
                .ForMember(dest => dest.reservation_id, opt => opt.Ignore())
                // Fields like member_id, book_id are set in the service, so we ignore mapping them from DTO.
                // The service creates the entity using the IDs passed in the method signature.
                .ForMember(dest => dest.member_id, opt => opt.Ignore())
                .ForMember(dest => dest.book_id, opt => opt.Ignore())
                .ForMember(dest => dest.reservation_date, opt => opt.Ignore())
                .ForMember(dest => dest.arrive_date, opt => opt.Ignore())
                .ForMember(dest => dest.status, opt => opt.Ignore())
                .ForMember(dest => dest.priority_number, opt => opt.Ignore());



            // =========================================================================
            // 11. REPORT 
            // =========================================================================
            CreateMap<REPORT, DashboardReportDto>()
                .ForMember(dest => dest.TotalBooks, opt => opt.MapFrom(src => src.TotalBooks))
                .ForMember(dest => dest.TotalCategories, opt => opt.MapFrom(src => src.TotalCategories))
                .ForMember(dest => dest.TotalMembers, opt => opt.MapFrom(src => src.TotalMembers))
                .ForMember(dest => dest.TotalBorrowingCount, opt => opt.MapFrom(src => src.TotalBorrowingCount))
                .ForMember(dest => dest.TotalActiveBorrowings, opt => opt.MapFrom(src => src.TotalActiveBorrowings));






        }
    }
}
