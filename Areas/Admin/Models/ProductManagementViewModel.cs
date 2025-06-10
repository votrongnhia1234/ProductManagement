using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Areas.Admin.Models
{
    public class ProductManagementViewModel
    {
        public List<ProductViewModel> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public int? CategoryFilter { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalProducts { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateProductViewModel
    {
        [Required]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Hình ảnh")]
        public IFormFile? ImageFile { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }

    public class EditProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Hình ảnh mới")]
        public IFormFile? ImageFile { get; set; }

        public string? CurrentImageUrl { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
