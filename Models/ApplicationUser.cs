using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public string FullName => $"{FirstName} {LastName}";
    }
}
