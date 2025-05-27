namespace ProductManagement.Models
{
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public string SortOrder { get; set; } = "name_asc";
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
    }
}
