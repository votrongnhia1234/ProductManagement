using ProductManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Areas.Admin.Models
{
    public class UserManagementViewModel
    {
        public List<UserViewModel> Users { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public string RoleFilter { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalUsers { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<string> Roles { get; set; } = new();
        public int OrderCount { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        public List<string> SelectedRoles { get; set; } = new();
        public List<string> AvailableRoles { get; set; } = new();
    }
}
