using System.ComponentModel.DataAnnotations;
using ProductManagement.Models;
using System.Collections.Generic;

namespace ProductManagement.Areas.Customer.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhận hàng là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Địa chỉ nhận hàng không được vượt quá {1} ký tự.")]
        [Display(Name = "Địa chỉ nhận hàng")]
        public string ShippingAddress { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá {1} ký tự.")]
        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; }
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }

    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
} 