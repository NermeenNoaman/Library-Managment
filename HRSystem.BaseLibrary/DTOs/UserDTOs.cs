// DTOs for TPLUser Entity (Authentication and Authorization)

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRSystem.BaseLibrary.DTOs
{
    // =================================================================================
    // 1. READ DTO (OUTPUT): Data sent to the Frontend (GET Requests)
    // =================================================================================
    public class UserReadDto
    {
        // Internal ID (PK)
        public int user_id { get; set; }
        public string email { get; set; }

        // Security Info (safe to expose)
        public string role { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }




        // Tokens Info
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpires { get; set; }
    }

    // =================================================================================
    // 2. REGISTER DTO (INPUT): Data received to create a new user (POST / Register)
    // =================================================================================
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "email is required.")]
        public string email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        // Custom validation for strong password (e.g., minimum length 8)
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [DataType(DataType.Password)]
        // Ensures Password and ConfirmPassword match before processing
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
        public string Role { get; set; } = "Member";
    }

    // =================================================================================
    // 3. LOGIN DTO (INPUT): Data received for authentication (POST / Login)
    // =================================================================================
    public class UserLoginDto
    {
        [Required(ErrorMessage = "email is required.")]
        [StringLength(50)]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
