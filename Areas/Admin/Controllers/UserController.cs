using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Areas.Admin.Models;
using ProductManagement.Models;

namespace ProductManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string searchTerm, string roleFilter, int page = 1)
        {
            const int pageSize = 10;

            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => u.Email.Contains(searchTerm) ||
                                        u.FirstName.Contains(searchTerm) ||
                                        u.LastName.Contains(searchTerm));
            }

            var totalUsers = users.Count();
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            var userList = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    Roles = roles.ToList(),
                    OrderCount = user.Orders?.Count ?? 0
                });
            }

            var viewModel = new UserManagementViewModel
            {
                Users = userViewModels,
                SearchTerm = searchTerm ?? string.Empty,
                RoleFilter = roleFilter ?? string.Empty,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalUsers = totalUsers
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = _roleManager.Roles.Select(r => r.Name!).ToList();
            var viewModel = new CreateUserViewModel
            {
                AvailableRoles = roles
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.SelectedRoles?.Any() == true)
                    {
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    }

                    TempData["Success"] = "Người dùng đã được tạo thành công.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.AvailableRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

            TempData["Success"] = $"Trạng thái người dùng đã được {(user.IsActive ? "kích hoạt" : "vô hiệu hóa")}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var orders = user.Orders?.Count ?? 0;

            var viewModel = new UserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Roles = roles.ToList(),
                OrderCount = orders
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name!).ToList();

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                IsActive = user.IsActive,
                SelectedRoles = roles.ToList(),
                AvailableRoles = allRoles
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user properties
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.IsActive = model.IsActive;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Update roles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var rolesToRemove = currentRoles.Except(model.SelectedRoles ?? new List<string>());
                    var rolesToAdd = (model.SelectedRoles ?? new List<string>()).Except(currentRoles);

                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    await _userManager.AddToRolesAsync(user, rolesToAdd);

                    TempData["Success"] = "Thông tin người dùng đã được cập nhật thành công.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.AvailableRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent deleting the current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id == user.Id)
            {
                TempData["Error"] = "Bạn không thể xóa tài khoản của chính mình.";
                return RedirectToAction(nameof(Index));
            }

            // Check if user has orders
            if (user.Orders?.Any() == true)
            {
                TempData["Error"] = "Không thể xóa người dùng có đơn hàng. Vui lòng vô hiệu hóa thay vì xóa.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Người dùng đã được xóa thành công.";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa người dùng.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Generate new password
            var newPassword = GenerateRandomPassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = $"Mật khẩu đã được reset thành công. Mật khẩu mới: {newPassword}";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi reset mật khẩu.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
