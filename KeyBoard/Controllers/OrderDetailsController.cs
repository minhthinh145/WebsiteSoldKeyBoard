using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _repo;
        private readonly IMapper _mapper;

        public OrderDetailsController(IOrderDetailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //get all OrderDetails by OrderId
        [HttpGet("ByOrder/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            var orderDetails = await _repo.GetOrderDetailsByOrderIdAsync(orderId);
            if (orderDetails == null)
            {
                return NotFound();
            }
            var orderDTOs = _mapper.Map<List<OrderDetailDTO>>(orderDetails);
            return Ok(orderDTOs);
        }

        // Get a single OrderDetail by Id
        [HttpGet("ById/{orderDetailId}")]
        public async Task<IActionResult> GetOrderDetailById(Guid orderDetailId)
        {
            var orderDetail = await _repo.GetOrderDetailByIdAsync(orderDetailId);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var orderDTO = _mapper.Map<OrderDetailDTO>(orderDetail);
            return Ok(orderDTO);
        }

        //Add a new OrderDetail
        [HttpPost]
        public async Task<IActionResult> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            var result = await _repo.AddOrderDetailAsync(orderDetail);
            if (result)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        //Update an OrderDetail
        [HttpPut]
        public async Task<IActionResult> UpdateOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            var result = await _repo.UpdateOrderDetailAsync(orderDetail);
            if (result)
            {
                return Ok(new { message = "OrderDetail updated successfully." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        //Delete an OrderDetail
        [HttpDelete("{orderDetailId}")]
        public async Task<IActionResult> RemoveOrderDetail(Guid orderDetailId)
        {
            var existingOrderDetail = await _repo.GetOrderDetailByIdAsync(orderDetailId);
            if (existingOrderDetail == null)
            {
                return NotFound(new { message = "Order detail not found." });
            }

            var result = await _repo.RemoveOrderDetailAsync(orderDetailId);
            if (result)
            {
                return Ok(new { message = "OrderDetail deleted successfully." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to delete order detail." });
        }
    }
}
