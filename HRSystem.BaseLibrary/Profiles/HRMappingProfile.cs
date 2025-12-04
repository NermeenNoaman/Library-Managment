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

            // Read: Convert Entity (DB) to ReadDto (Output to Frontend)
            CreateMap<USER, UserReadDto>();

            // Create: Convert CreateDto (Input) to Entity (for adding new record)
            CreateMap<USER, UserRegisterDto>();

            // Update: Convert UpdateDto (Input, includes ID) to Entity (for modifying existing record)
            CreateMap<USER, UserLoginDto>();


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
                .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => DateTime.Now)); // Set creation date

            // 3. Update Mapping: DTO to Entity (Allows partial updates)
            CreateMap<LibraryBranchUpdateDto, LIBRARY_BRANCH>()
                .ForMember(dest => dest.updated_at, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null source values



            // =========================================================================
            // 4. DTOs for TPLUser (Authentication - Keep simple for Security Team)
            // =========================================================================


            // =========================================================================
            // 5. DTOs for TPLRequests (Leave Requests)
            // =========================================================================


            // =========================================================================
            // 6. DTOs for TPLLeaveBalance (Employee Leave Balances)
            // =========================================================================




            // =========================================================================
            // 7. DTOs for LKPLeaveType (Leave Type - CRUD) - NEW MAPPING
            // =========================================================================


            // =========================================================================
            // 8. DTOs for TPLLeave (Leave Log) - NEW MAPPING
            // =========================================================================


            // =========================================================================
            // 9. DTOs for TPLEmployee (Employee)
            // =========================================================================


            // =========================================================================
            // 10. Responsible for the process of converting data between DTOs and Entities
            // =========================================================================



            // =========================================================================
            // 11. TPLLeave MAPPINGS (Leave Log)
            // =========================================================================







            // =========================================================================
            // 12. PermissionType MAPPINGS
            // =========================================================================





        }
    }
}
