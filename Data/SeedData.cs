using Microsoft.AspNetCore.Identity;
using ProductManagement.Models;

namespace ProductManagement.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure roles exist
            string[] roleNames = { "Admin", "Manager", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminEmail = "admin@productmanagement.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create manager user
            var managerEmail = "manager@productmanagement.com";
            var managerUser = await userManager.FindByEmailAsync(managerEmail);

            if (managerUser == null)
            {
                managerUser = new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FirstName = "Product",
                    LastName = "Manager",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(managerUser, "Manager@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(managerUser, "Manager");
                }
            }

            // Add Categories
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Điện tử", Description = "Thiết bị và tiện ích điện tử" },
                    new Category { Name = "Sách", Description = "Nhiều loại sách và tiểu thuyết" },
                    new Category { Name = "Thời trang", Description = "Quần áo và các mặt hàng thời trang" },
                    new Category { Name = "Nhà cửa & Vườn", Description = "Đồ dùng cho nhà cửa và sân vườn" }
                );
                await context.SaveChangesAsync();
            }

            // Add Products
            if (!context.Products.Any())
            {
                var electronicsCategory = context.Categories.FirstOrDefault(c => c.Name == "Điện tử");
                var booksCategory = context.Categories.FirstOrDefault(c => c.Name == "Sách");
                var clothingCategory = context.Categories.FirstOrDefault(c => c.Name == "Thời trang");

                if (electronicsCategory != null)
                {
                    context.Products.AddRange(
                        new Product { ProductName = "Máy tính xách tay", Description = "Máy tính xách tay hiệu năng cao", Price = 1200.00M, CategoryId = electronicsCategory.Id, ImgUrl = "https://via.placeholder.com/300x200?text=Laptop" },
                        new Product { ProductName = "Điện thoại thông minh", Description = "Điện thoại thông minh mẫu mới nhất", Price = 800.00M, CategoryId = electronicsCategory.Id, ImgUrl = "https://via.placeholder.com/300x200?text=Smartphone" }
                    );
                }

                if (booksCategory != null)
                {
                    context.Products.AddRange(
                        new Product { ProductName = "Chúa tể những chiếc nhẫn", Description = "Tiểu thuyết giả tưởng sử thi", Price = 20.00M, CategoryId = booksCategory.Id, ImgUrl = "https://via.placeholder.com/300x200?text=Book" },
                        new Product { ProductName = "Xứ Cát", Description = "Khoa học viễn tưởng kinh điển", Price = 15.00M, CategoryId = booksCategory.Id, ImgUrl = "https://via.placeholder.com/300x200?text=Book" }
                    );
                }

                if (clothingCategory != null)
                {
                    context.Products.AddRange(
                        new Product { ProductName = "Áo phông", Description = "Áo phông cotton thoải mái", Price = 25.00M, CategoryId = clothingCategory.Id, ImgUrl = "https://via.placeholder.com/300x200?text=T-Shirt" },
                        new Product { ProductName = "Quần jean", Description = "Quần jean vải bền", Price = 50.00M, CategoryId = clothingCategory.Id, ImgUrl = "https://via.placeholder.com/300x200?text=Jeans" }
                    );
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
