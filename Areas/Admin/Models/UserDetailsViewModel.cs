using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Areas.Admin.Models
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<string> Roles { get; set; } = new();
        public int OrderCount { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<UserActivityViewModel> RecentActivities { get; set; } = new();
        public decimal TotalSpent { get; set; }
        public int ProductsManaged { get; set; }
        public int ReviewsWritten { get; set; }
        public string? LastLoginIP { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";

        [StringLength(500)]
        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public List<string> SelectedRoles { get; set; } = new();
        public List<string> AvailableRoles { get; set; } = new();

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UserActivityViewModel
    {
        public DateTime Date { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
