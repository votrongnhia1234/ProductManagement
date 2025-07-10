using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using System.Diagnostics;

namespace ProductManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Nếu đã đăng nhập và là Admin thì chuyển về dashboard admin
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            // Còn lại (chưa đăng nhập hoặc Customer) thì chuyển về trang cửa hàng
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
