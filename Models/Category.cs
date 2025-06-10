using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
