using ProductManagement.Areas.Customer.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Previewer;
using System.IO;
using ProductManagement.Models;

namespace ProductManagement.Services
{
    public class OrderInvoicePdfService
    {
        private string GetOrderStatusText(object status)
        {
            // Nếu status là enum OrderStatus, chuyển sang string
            var statusStr = status.ToString();
            return statusStr switch
            {
                "Pending" => "Chờ xử lý",
                "Confirmed" => "Đã xác nhận",
                "Processing" => "Đang xử lý",
                "Shipped" => "Đã gửi hàng",
                "Delivered" => "Đã giao hàng",
                "Cancelled" => "Đã hủy",
                "Returned" => "Đã trả hàng",
                _ => "Không xác định"
            };
        }

        public byte[] GenerateInvoice(OrderConfirmationViewModel order, string customerName, string customerEmail)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("HÓA ĐƠN BÁN HÀNG").SemiBold().FontSize(20).AlignCenter();
                    page.Content().Column(col =>
                    {
                        //col.Item().Text($"Mã đơn hàng: {order.OrderId}");
                        //col.Item().Text($"Ngày đặt: {order.OrderDate:dd/MM/yyyy}");
                        col.Item().PaddingTop(8).Text("THÔNG TIN KHÁCH HÀNG").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().PaddingTop(8).Text($"Họ tên: {customerName}").FontSize(10);
                        col.Item().Text($"Email: {customerEmail}").FontSize(10);
                        col.Item().Text($"Điện thoại: 0123456789").FontSize(10);
                        col.Item().PaddingTop(5).Text("ĐỊA CHỈ GIAO HÀNG").FontSize(10).Bold().FontColor(Colors.Blue.Darken1);
                        col.Item().Text($"{order.ShippingAddress}").FontSize(10);

                        // Thông tin đơn hàng
                        col.Item().PaddingTop(15).Text("THÔNG TIN ĐƠN HÀNG").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().Text($"Mã đơn hàng: #{order.OrderId:D6}").FontSize(10);
                        col.Item().Text($"Ngày đặt: {order.OrderDate:dd/MM/yyyy HH:mm}").FontSize(10);
                        col.Item().Text($"Trạng thái: {GetOrderStatusText(order.Status)}").FontSize(10);

                        // if (!string.IsNullOrEmpty(order.TrackingNumber))
                        // {
                        //     col.Item().Text($"Mã vận đơn: {order.TrackingNumber}").FontSize(10);
                        // }

                        // col.Item().PaddingTop(5).Text("PHƯƠNG THỨC THANH TOÁN").FontSize(10).Bold().FontColor(Colors.Blue.Darken1);
                        // col.Item().Text("Thanh toán online").FontSize(10);

                        // Spacing
                        col.Item().PaddingVertical(10);

                        // Bảng sản phẩm
                        col.Item().Text("CHI TIẾT ĐƠN HÀNG").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);

                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // STT
                                columns.RelativeColumn(3);   // Tên sản phẩm
                                columns.ConstantColumn(60);  // Số lượng
                                columns.ConstantColumn(100); // Đơn giá
                                columns.ConstantColumn(120); // Thành tiền
                            });

                            // Header của bảng
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("STT").FontColor(Colors.White).Bold().AlignCenter();
                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("TÊN SẢN PHẨM").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("SỐ LƯỢNG").FontColor(Colors.White).Bold().AlignCenter();
                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("ĐƠN GIÁ").FontColor(Colors.White).Bold().AlignRight();
                                header.Cell().Background(Colors.Blue.Darken2).Padding(8).Text("THÀNH TIỀN").FontColor(Colors.White).Bold().AlignRight();
                            });

                            // Dữ liệu sản phẩm
                            int stt = 1;
                            foreach (var item in order.Items)
                            {
                                var backgroundColor = stt % 2 == 0 ? Colors.Grey.Lighten5 : Colors.White;

                                table.Cell().Background(backgroundColor).Padding(8).Text(stt.ToString()).AlignCenter();
                                table.Cell().Background(backgroundColor).Padding(8).Text(item.ProductName).FontSize(10);
                                table.Cell().Background(backgroundColor).Padding(8).Text(item.Quantity.ToString()).AlignCenter();
                                table.Cell().Background(backgroundColor).Padding(8).Text($"{item.Price:N0} USD").AlignRight();
                                table.Cell().Background(backgroundColor).Padding(8).Text($"{item.TotalPrice:N0} USD").AlignRight().Bold();

                                stt++;
                            }
                        });

                        // Tổng cộng
                        col.Item().PaddingTop(20).AlignRight().Column(subCol =>
                        {
                            subCol.Item().Background(Colors.Blue.Lighten4).Padding(15).Row(row =>
                            {
                                row.RelativeItem().Text("TỔNG CỘNG:").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
                                row.ConstantItem(150).Text($"{order.TotalAmount:N0} USD").FontSize(16).Bold().FontColor(Colors.Red.Darken1).AlignRight();
                            });

                            subCol.Item().PaddingTop(5).Text($"Bằng chữ: {ConvertNumberToWords(order.TotalAmount)} USD")
                                .FontSize(10).Italic().FontColor(Colors.Grey.Darken1);
                        });

                        // Ghi chú
                        col.Item().PaddingTop(30).Column(subCol =>
                        {
                            subCol.Item().Text("GHI CHÚ & ĐIỀU KHOẢN").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
                            subCol.Item().PaddingTop(5).Text("• Hóa đơn này được tạo tự động bởi hệ thống").FontSize(9);
                            subCol.Item().Text("• Quý khách vui lòng kiểm tra kỹ sản phẩm khi nhận hàng").FontSize(9);
                            subCol.Item().Text("• Đổi trả trong vòng 7 ngày với sản phẩm còn nguyên seal").FontSize(9);
                            subCol.Item().Text("• Liên hệ hotline 1900-1234 để được hỗ trợ").FontSize(9);
                        });

                        // Chữ ký
                        col.Item().PaddingTop(40).Row(row =>
                        {
                            row.RelativeItem().Column(subCol =>
                            {
                                subCol.Item().Text("KHÁCH HÀNG").FontSize(10).Bold().AlignCenter();
                                subCol.Item().Text("(Ký, ghi rõ họ tên)").FontSize(8).AlignCenter().Italic();
                                subCol.Item().PaddingTop(40).Text("").AlignCenter(); // Space for signature
                            });

                            row.RelativeItem().Column(subCol =>
                            {
                                subCol.Item().Text($"TP.HCM, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}")
                                    .FontSize(10).AlignCenter();
                                subCol.Item().Text("NGƯỜI BÁN HÀNG").FontSize(10).Bold().AlignCenter();
                                subCol.Item().Text("(Ký, đóng dấu)").FontSize(8).AlignCenter().Italic();
                                subCol.Item().PaddingTop(40).Text("").AlignCenter(); // Space for signature
                            });
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }

        private string ConvertNumberToWords(decimal number)
        {
            // Simplified number to words conversion for Vietnamese
            // You can implement a more sophisticated version
            var intPart = (long)number;

            if (intPart == 0) return "Không";

            string[] ones = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] tens = { "", "", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
            string[] scales = { "", "nghìn", "triệu", "tỷ" };

            if (intPart < 10)
                return char.ToUpper(ones[intPart][0]) + ones[intPart].Substring(1);

            if (intPart < 100)
            {
                var ten = intPart / 10;
                var one = intPart % 10;
                if (ten == 1)
                    return "Mười" + (one > 0 ? " " + ones[one] : "");
                return char.ToUpper(tens[ten][0]) + tens[ten].Substring(1) + (one > 0 ? " " + ones[one] : "");
            }

            // For larger numbers, return a simplified version
            if (intPart < 1000)
                return $"{ConvertNumberToWords(intPart / 100)} trăm" + (intPart % 100 > 0 ? " " + ConvertNumberToWords(intPart % 100) : "");

            if (intPart < 1000000)
                return $"{ConvertNumberToWords(intPart / 1000)} nghìn" + (intPart % 1000 > 0 ? " " + ConvertNumberToWords(intPart % 1000) : "");

            return $"{ConvertNumberToWords(intPart / 1000000)} triệu" + (intPart % 1000000 > 0 ? " " + ConvertNumberToWords(intPart % 1000000) : "");
        }
    }
}