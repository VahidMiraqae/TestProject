using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProject.Application;
using TestProject.Application.Dtos;
using TestProject.Core; 

namespace TestProject.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto placeOrderDto)
        {
            var order = Order.NewOrder();
            order = _mapper.Map<PlaceOrderDto,Order>(placeOrderDto,order); 
            order = await _orderService.CreateAsync(order);
            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }
    }
}
