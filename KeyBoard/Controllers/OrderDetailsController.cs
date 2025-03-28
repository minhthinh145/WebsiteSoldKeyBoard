using KeyBoard.DTOs;
using KeyBoard.Helpers;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("ByOrder/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            var orderDetails = await _service.GetOrderDetailsByOrderIdAsync(orderId);

            return Ok(orderDetails);
        }

        // Get a single OrderDetail by Id
        [Authorize]
        [HttpGet("ById/{orderDetailId}")]
        public async Task<IActionResult> GetOrderDetailById(Guid orderDetailId)
        {
           var orderDetail = await _service.GetOrderDetailByIdAsync(orderDetailId);
           return Ok(orderDetail);
        }

        //Add a new OrderDetail
        [Authorize(Roles = ApplicationRole.Admin)]
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
        [Authorize(Roles = ApplicationRole.Admin)]
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
        [Authorize(Roles = ApplicationRole.Admin)]
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
