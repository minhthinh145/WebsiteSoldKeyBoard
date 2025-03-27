using KeyBoard.DTOs.VNPayDTOs;

namespace KeyBoard.Services.VNPayServices
{
    public interface IVNPayService
    {
        /// <summary>
        /// Tạo URL thanh toán VNPay dựa trên mã hóa đơn.
        /// </summary>
        /// <param name="maHD">Mã hóa đơn</param>
        /// <param name="httpContext">HttpContext để lấy thông tin request</param>
        /// <returns>URL để redirect người dùng đến VNPay</returns>
        Task<string> CreatePaymentUrl(int maHD, HttpContext httpContext);

        /// <summary>
        /// Xử lý phản hồi từ VNPay sau khi thanh toán.
        /// </summary>
        /// <param name="queryParams">Danh sách query string từ VNPay gửi về</param>
        /// <returns>Thông tin phản hồi thanh toán</returns>
        VNPayResponseDTO ProcessPaymentResponse(IQueryCollection queryParams);
    }
}
