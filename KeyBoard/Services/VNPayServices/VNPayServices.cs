using AutoMapper;
using KeyBoard.DTOs.HoaDonsDTOs;
using KeyBoard.DTOs.VNPayDTOs;
using KeyBoard.Helpers;
using KeyBoard.Repositories.Interfaces;
using System.Threading.Tasks;
using static KeyBoard.Helpers.VNPayHelper;

namespace KeyBoard.Services.VNPayServices
{
    public class VNPayServices : IVNPayService
    {
        private readonly IConfiguration _config;
        private readonly IHoaDonRepository _hoadon;
        private readonly IMapper _mapper;

        public VNPayServices(IConfiguration config, IHoaDonRepository hoadon, IMapper mapper)
        {
            _config = config;
            _hoadon = hoadon;
            _mapper = mapper;
        }

        /// <summary>
        /// Tạo URL thanh toán VNPay
        /// </summary>  
        public async Task<string> CreatePaymentUrl(int maHD, HttpContext context)
        {
            var hoadon = await _hoadon.GetHoaDonByIdAsync(maHD);
            if (hoadon == null) throw new Exception("Hóa đơn không tồn tại");
            var hoadonDTO = _mapper.Map<HoaDonDTO>(hoadon);
            double tongTien = Convert.ToDouble(hoadonDTO.ChiTietHoaDons.Sum(ct => ct.SoLuong * ct.DonGia));
            //tạo model cho request
            var model = new VNPayRequestDTO
            {
                Amount = tongTien,
                MaHd = hoadon.MaHd
            };
            var tick = DateTime.Now.Ticks.ToString();

            var pay = new VNPayHelper();
            pay.AddRequestData("vnp_Version", _config["VNPay:Version"]);
            pay.AddRequestData("vnp_Command", _config["VNPay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _config["VNPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (model.Amount).ToString());
            pay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _config["VNPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr",    Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _config["VNPay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.MaHd);
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", _config["VNPay:ReturnUrl"]);
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl = pay.CreatePaymentUrl(_config["VNPay:BaseUrl"], _config["VNPay:HashSecret"]);
            return paymentUrl;
        }

        /// <summary>
        /// Xử lý phản hồi từ VNPay
        /// </summary>
        public VNPayResponseDTO ProcessPaymentResponse(IQueryCollection query)
        {
            var vnpay = new VNPayHelper();
            foreach (var (key, value) in query)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var vnp_orderid = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = query.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool CheckSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VNPay:HashSecret"]);
            if (!CheckSignature)
            {
                return new VNPayResponseDTO
                {
                    Success = false
                };
            }
            return new VNPayResponseDTO
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                PaymentId = vnp_orderid.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode.ToString()
            };
        }
    }
}
