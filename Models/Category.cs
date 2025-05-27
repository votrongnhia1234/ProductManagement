using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Models
{
    public class Category
    {
        public int Id { get; set; }
    
    [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<Product>? Products { get; set; }
    
    // Add this property for performance
    [NotMapped]
    public int ProductCount { get; set; }
    }
}
