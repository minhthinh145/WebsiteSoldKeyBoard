using KeyBoard.DTOs.VNPayDTOs;

namespace KeyBoard.Services.VNPayServices
{
    public interface IVNPayService
    {
        /// <summary>
        /// Tạo URL thanh toán VNPay dựa trên thông tin đơn hàng.
        /// </summary>
        /// <param name="request">Thông tin thanh toán</param>
        /// <returns>URL để redirect người dùng đến VNPay</returns>
        string CreatePaymentUrl(VNPayRequestDTO request, HttpContext httpContext);

        /// <summary>
        /// Xử lý phản hồi từ VNPay sau khi thanh toán.
        /// </summary>
        /// <param name="queryParams">Danh sách query string từ VNPay gửi về</param>
        /// <returns>Thông tin phản hồi thanh toán</returns>
        VNPayResponseDTO ProcessPaymentResponse(IQueryCollection queryParams);
    }
}
