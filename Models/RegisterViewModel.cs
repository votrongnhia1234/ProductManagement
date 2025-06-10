using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Họ là bắt buộc.")]
        [Display(Name = "Họ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [Display(Name = "Tên")]
        public string LastName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }
    }
} 