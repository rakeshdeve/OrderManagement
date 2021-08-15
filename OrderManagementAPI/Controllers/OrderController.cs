using Microsoft.AspNetCore.Mvc;
using OrderManagement.Data.IRepository;
using OrderManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        //Declaration
        private IOrderRepository _orderRepository;

        //Constructor Injection
        public OrderController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        #region Get Order List
        [HttpGet("GetOrderList")]
        public async Task<IActionResult> GetOrderList()
        {
            try
            {
                return Ok(await _orderRepository.GetOrdersList());
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Save Order
        [HttpPost("SaveOrder")]
        public async Task<IActionResult> SaveOrder(Order orderRequest)
        {
            try
            {
                if (orderRequest == null)
                {
                    return BadRequest();
                }
                else
                {
                    var response = await _orderRepository.SaveOrder(orderRequest);
                    if (response != null && response.Status == "SUCCESS")
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(response);
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Update Order
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(Order orderRequest)
        {
            try
            {
                if (orderRequest == null || (orderRequest != null && orderRequest.OrderId == 0))
                {
                    return BadRequest();
                }
                else
                {
                    var response = await _orderRepository.UpdateOrder(orderRequest);
                    if (response != null && response.Status == "SUCCESS")
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(response);
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Get Order By Id
        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(long orderId)
        {
            try
            {
                if (orderId > 0)
                {
                    return Ok(await _orderRepository.GetOrderById(orderId));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Delete Orders
        [HttpPost("DeleteOrders")]
        public async Task<IActionResult> DeleteOrders(DeleteOrder deleteRequest)
        {
            try
            {
                if (deleteRequest != null && deleteRequest.OrderIds != null && deleteRequest.OrderIds.Length > 0)
                {
                    var response = await _orderRepository.DeleteOrderById(deleteRequest.OrderIds);
                    if (response != null && response.Status == "SUCCESS")
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
