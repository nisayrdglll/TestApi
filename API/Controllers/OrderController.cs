using Microsoft.AspNetCore.Mvc;
using DataAccess.Context;
using DataAccess.Schemas;
using DataAccess.DTOs;
using Core.Services;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly DbContext _context;
        private readonly OrderService _orderService;

        public OrderController(ILogger<OrderController> logger, DbContext context)
        {
            _logger = logger;
            _context = context;
            _orderService = new OrderService(_context);
        }

        /// <summary>
        /// Yeni sipariş oluşturur
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DefaultAPIResponseSchema>> CreateOrder([FromBody] CreateOrderRequestDTO request)
        {
            var defaultResponse = new DefaultAPIResponseSchema
            {
                Meta = new APIMetaInformationSchema(),
                Errors = new List<ErrorSchema>()
            };

            try
            {
                if (!ModelState.IsValid)
                {
                    defaultResponse.Meta.Successful = false;
                    defaultResponse.Meta.ErrorCount = ModelState.ErrorCount;
                    defaultResponse.Meta.StatusCode = 400;
                    defaultResponse.Meta.Status = "BADREQUEST";

                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        defaultResponse.Errors.Add(new ErrorSchema
                        {
                            Field = "",
                            Message = error.ErrorMessage
                        });
                    }
                    return BadRequest(defaultResponse);
                }

                var order = await _orderService.CreateOrderAsync(request);

                var orderResponse = new OrderResponseDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    Notes = order.Notes,
                    CreatedAt = order.CreatedAt,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDTO
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                };

                defaultResponse.Meta.Successful = true;
                defaultResponse.Meta.ErrorCount = 0;
                defaultResponse.Meta.PageCount = 1;
                defaultResponse.Meta.StatusCode = 201;
                defaultResponse.Meta.Status = "CREATED";
                defaultResponse.Meta.Limit = 1;
                defaultResponse.Items = new List<object> { orderResponse };

                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, defaultResponse);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid order request");
                defaultResponse.Meta.Successful = false;
                defaultResponse.Meta.ErrorCount = 1;
                defaultResponse.Meta.StatusCode = 400;
                defaultResponse.Meta.Status = "BADREQUEST";
                defaultResponse.Errors.Add(new ErrorSchema
                {
                    Field = "",
                    Message = ex.Message
                });
                return BadRequest(defaultResponse);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Stock insufficient for order");
                defaultResponse.Meta.Successful = false;
                defaultResponse.Meta.ErrorCount = 1;
                defaultResponse.Meta.StatusCode = 409;
                defaultResponse.Meta.Status = "CONFLICT";
                defaultResponse.Errors.Add(new ErrorSchema
                {
                    Field = "",
                    Message = ex.Message
                });
                return Conflict(defaultResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                defaultResponse.Meta.Successful = false;
                defaultResponse.Meta.ErrorCount = 1;
                defaultResponse.Meta.StatusCode = 500;
                defaultResponse.Meta.Status = "INTERNALSERVERERROR";
                defaultResponse.Errors.Add(new ErrorSchema
                {
                    Field = "",
                    Message = "Sipariş oluşturulurken hata oluştu."
                });
                return StatusCode(500, defaultResponse);
            }
        }

        /// <summary>
        /// Kullanıcının siparişlerini listeler
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<DefaultAPIResponseSchema>> GetOrdersByUserId(
            string userId,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            var defaultResponse = new DefaultAPIResponseSchema
            {
                Meta = new APIMetaInformationSchema(),
                Errors = new List<ErrorSchema>()
            };

            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId, page, limit);
                var totalCount = await _orderService.GetOrderCountByUserIdAsync(userId);

                defaultResponse.Meta.Page = page;
                defaultResponse.Meta.Limit = limit;
                defaultResponse.Meta.Count = totalCount;
                defaultResponse.Meta.PageCount = orders.Count;

                if (orders.Count == 0)
                {
                    defaultResponse.Meta.Successful = false;
                    defaultResponse.Meta.ErrorCount = 1;
                    defaultResponse.Meta.StatusCode = 404;
                    defaultResponse.Meta.Status = "NOTFOUND";
                    defaultResponse.Errors.Add(new ErrorSchema
                    {
                        Field = "",
                        Message = "Sipariş bulunamadı."
                    });
                    return NotFound(defaultResponse);
                }

                defaultResponse.Meta.Successful = true;
                defaultResponse.Meta.ErrorCount = 0;
                defaultResponse.Meta.StatusCode = 200;
                defaultResponse.Meta.Status = "OK";
                defaultResponse.Items = orders.Cast<object>().ToList();

                return Ok(defaultResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting orders for user {UserId}", userId);
                defaultResponse.Meta.Successful = false;
                defaultResponse.Meta.ErrorCount = 1;
                defaultResponse.Meta.StatusCode = 500;
                defaultResponse.Meta.Status = "INTERNALSERVERERROR";
                defaultResponse.Errors.Add(new ErrorSchema
                {
                    Field = "",
                    Message = "Siparişler getirilirken hata oluştu."
                });
                return StatusCode(500, defaultResponse);
            }
        }

        /// <summary>
        /// Sipariş detayını getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DefaultAPIResponseSchema>> GetOrderById(long id)
        {
            var defaultResponse = new DefaultAPIResponseSchema
            {
                Meta = new APIMetaInformationSchema(),
                Errors = new List<ErrorSchema>()
            };

            try
            {
                var order = await _orderService.GetOrderDetailsByIdAsync(id);

                if (order == null)
                {
                    defaultResponse.Meta.Successful = false;
                    defaultResponse.Meta.ErrorCount = 1;
                    defaultResponse.Meta.StatusCode = 404;
                    defaultResponse.Meta.Status = "NOTFOUND";
                    defaultResponse.Errors.Add(new ErrorSchema
                    {
                        Field = "",
                        Message = "Sipariş bulunamadı."
                    });
                    return NotFound(defaultResponse);
                }

                defaultResponse.Meta.Successful = true;
                defaultResponse.Meta.ErrorCount = 0;
                defaultResponse.Meta.PageCount = 1;
                defaultResponse.Meta.StatusCode = 200;
                defaultResponse.Meta.Status = "OK";
                defaultResponse.Meta.Limit = 1;
                defaultResponse.Items = new List<object> { order };

                return Ok(defaultResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting order {OrderId}", id);
                defaultResponse.Meta.Successful = false;
                defaultResponse.Meta.ErrorCount = 1;
                defaultResponse.Meta.StatusCode = 500;
                defaultResponse.Meta.Status = "INTERNALSERVERERROR";
                defaultResponse.Errors.Add(new ErrorSchema
                {
                    Field = "",
                    Message = "Sipariş detayları getirilirken hata oluştu."
                });
                return StatusCode(500, defaultResponse);
            }
        }

        /// <summary>
        /// Siparişi siler (sadece Pending durumundaki siparişler)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DefaultAPIResponseSchema>> DeleteOrder(long id)
        {
            var defaultResponse = new DefaultAPIResponseSchema
            {
                Meta = new APIMetaInformationSchema(),
                Errors = new List<ErrorSchema>()
            };

            try
            {
                var result = await _orderService.DeleteOrderAsync(id);

                if (!result)
                {
                    defaultResponse.Meta.Successful = false;
                    defaultResponse.Meta.ErrorCount = 1;
                    defaultResponse.Meta.StatusCode = 404;
                    defaultResponse.Meta.Status = "NOTFOUND";
                    defaultResponse.Errors.Add(new ErrorSchema
                    {
                        Field = "",
                        Message = "Sipariş bulunamadı."
                    });
                    return NotFound(defaultResponse);
                }

                defaultResponse.Meta.Successful = true;
                defaultResponse.Meta.ErrorCount = 0;
                defaultResponse.Meta.PageCount = 0;
                defaultResponse.Meta.StatusCode = 200;
                defaultResponse.Meta.Status = "OK";
                defaultResponse.Items = new List<object>();

                return Ok(defaultResponse);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Cannot delete order {OrderId}", id);
                defaultResponse.Meta.Successful = false;
                defaultResponse.Meta.ErrorCount = 1;
                defaultResponse.Meta.StatusCode = 409;
                defaultResponse.Meta.Status = "CONFLICT";
                defaultResponse.Errors.Add(new ErrorSchema
                {
                    Field = "",
                    Message = ex.Message
                });
                return Conflict(defaultResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting order {OrderId}", id);
                defaultResponse.Meta.Successful = false;
                defaultResponse.Meta.ErrorCount = 1;
                defaultResponse.Meta.StatusCode = 500;
                defaultResponse.Meta.Status = "INTERNALSERVERERROR";
                defaultResponse.Errors.Add(new ErrorSchema
                {
                    Field = "",
                    Message = "Sipariş silinirken hata oluştu."
                });
                return StatusCode(500, defaultResponse);
            }
        }
    }
}