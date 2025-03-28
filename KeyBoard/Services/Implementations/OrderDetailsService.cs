using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;

namespace KeyBoard.Services.Implementations
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IOrderDetailRepository _repo;
        private readonly IMapper _mapper;

        public OrderDetailsService(IOrderDetailRepository repo , IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> AddOrderDetailAsync(OrderDetailDTO orderDetailDTO)
        {
            if (orderDetailDTO == null) 
            {
                throw new ArgumentNullException(nameof(orderDetailDTO), "Dữ liệu không được null");
            }
            if (orderDetailDTO.Quantity <= 0 || orderDetailDTO.UnitPrice < 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0 và đơn giá không được âm.");
            }
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            var result = await _repo.AddOrderDetailAsync(orderDetail);
            return result;
        }

        public async Task<bool> CheckProductInOrderAsync(Guid orderId, Guid productId)
        {
            var orderDetails = await GetOrderDetailsByOrderIdAsync(orderId);
            return orderDetails.Any(x => x.ProductId == productId);
        }

        public async Task<OrderDetailDTO?> GetOrderDetailByIdAsync(Guid orderDetailId)
        {
            var orderDetail = await _repo.GetOrderDetailByIdAsync(orderDetailId);
            return orderDetail == null ? null : _mapper.Map<OrderDetailDTO>(orderDetail);
        }

        public async Task<List<OrderDetailDTO>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            var orderDetails = await _repo.GetOrderDetailsByOrderIdAsync(orderId);
            if (orderDetails == null) 
            {
                return new List<OrderDetailDTO>();
            }
            return _mapper.Map<List<OrderDetailDTO>>(orderDetails);
        }

        public async Task<decimal> GetTotalPriceByOrderIdAsync(Guid orderId)
        {
            var orderDetails = await GetOrderDetailsByOrderIdAsync(orderId);
            return orderDetails.Sum(od => od.UnitPrice * od.Quantity);
        }

        public async Task<bool> RemoveOrderDetailAsync(Guid orderDetailId)
        {
            try
            {
                // Kiểm tra OrderDetail có tồn tại không
                var exists = await GetOrderDetailByIdAsync(orderDetailId);
                if (exists == null)
                {
                    return false;
                }

                
                var result = await _repo.RemoveOrderDetailAsync(orderDetailId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa OrderDetail: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO)
        {
            try
            {
                if (orderDetailDTO == null)
                {
                    throw new ArgumentNullException(nameof(orderDetailDTO), "Dữ liệu không được null");
                }

                if (orderDetailDTO.Quantity <= 0 || orderDetailDTO.UnitPrice < 0)
                {
                    throw new ArgumentException("Số lượng phải lớn hơn 0 và đơn giá không được âm.");
                }

                var exists = await GetOrderDetailByIdAsync(orderDetailDTO.Id);
                if (exists == null)
                {
                    return false;
                }

                var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
                var result = await _repo.UpdateOrderDetailAsync(orderDetail);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật OrderDetail: {ex.Message}");
                return false;
            }
        }

    }
}
