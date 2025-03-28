using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _service;

        public OrderDetailsController(IOrderDetailsService service)
        {
            _service = service;
        }

        //get all OrderDetails by OrderId
        [HttpGet("ByOrder/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            var orderDetails = await _service.GetOrderDetailsByOrderIdAsync(orderId);

            return Ok(orderDetails);
        }

        // Get a single OrderDetail by Id
        [HttpGet("ById/{orderDetailId}")]
        public async Task<IActionResult> GetOrderDetailById(Guid orderDetailId)
        {
           var orderDetail = await _service.GetOrderDetailByIdAsync(orderDetailId);
           return Ok(orderDetail);
        }

        //Add a new OrderDetail
        [HttpPost]
        public async Task<IActionResult> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _service.AddOrderDetailAsync(orderDetailDTO);
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
            var result = await _service.UpdateOrderDetailAsync(orderDetailDTO);
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
            var result = await _service.RemoveOrderDetailAsync(orderDetailId);
            if (result)
            {
                return Ok(new { message = "OrderDetail deleted successfully." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to delete order detail." });
        }
    }
}
