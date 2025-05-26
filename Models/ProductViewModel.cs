using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ProductManagement.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public int? CategoryFilter { get; set; }
        public string SortOrder { get; set; } = "name_asc";
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 6;
    }
}
