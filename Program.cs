using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Repositories;
using ProductManagement.Repositories.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
    
    // Enable sensitive data logging in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

var app = builder.Build();

// Tạo database và seed data nếu cần
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("=== DATABASE SETUP ===");
        logger.LogInformation("Connection String: {ConnectionString}", 
            builder.Configuration.GetConnectionString("DefaultConnection"));
        
        // Sử dụng migration thay vì EnsureCreated
        logger.LogInformation("Running database migrations...");
        context.Database.Migrate();
        
        // Kiểm tra dữ liệu chi tiết
        var productCount = context.Products.Count();
        var categoryCount = context.Categories.Count();
        
        logger.LogInformation("=== DATABASE CONTENT ===");
        logger.LogInformation("Database contains {ProductCount} products and {CategoryCount} categories", 
            productCount, categoryCount);
        
        // Log tất cả sản phẩm trong database
        var allProducts = context.Products.OrderBy(p => p.Id).ToList();
        logger.LogInformation("=== ALL PRODUCTS IN DATABASE ===");
        foreach (var product in allProducts)
        {
            logger.LogInformation("DB Product: ID={Id}, Name={Name}, Price={Price:C}", 
                product.Id, product.ProductName, product.Price);
        }
        logger.LogInformation("=== END DATABASE PRODUCTS ===");
        
        // Kiểm tra repository type
        var productRepo = services.GetRequiredService<IProductRepository>();
        var categoryRepo = services.GetRequiredService<ICategoryRepository>();
        
        logger.LogInformation("=== REPOSITORY TYPES ===");
        logger.LogInformation("Product Repository Type: {Type}", productRepo.GetType().FullName);
        logger.LogInformation("Category Repository Type: {Type}", categoryRepo.GetType().FullName);
        
        // Test repository
        var repoProducts = await productRepo.GetAllAsync();
        logger.LogInformation("Repository returns {Count} products", repoProducts.Count);
        
        foreach (var product in repoProducts.Take(3))
        {
            logger.LogInformation("Repo Product: ID={Id}, Name={Name}, Price={Price:C}", 
                product.Id, product.ProductName, product.Price);
        }
        
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "=== DATABASE SETUP ERROR ===");
        logger.LogError(ex, "Error details: {Message}", ex.Message);
        if (ex.InnerException != null)
        {
            logger.LogError(ex.InnerException, "Inner exception: {Message}", ex.InnerException.Message);
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
