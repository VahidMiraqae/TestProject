using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestProject.Application.Dtos;
using TestProject.Core;
using TestProject.RabbitMq;

namespace TestProject.Application
{
    public class OrderService : IOrderService
    {
        private IMessageProducer _messageProducer;
        private IOrderRepository _orderRepository;
        private IOrderPolicy _orderPolicy;
        private IHttpClientFactory _httpClientFactory;
        private IMapper _mapper;

        public OrderService(IMessageProducer messageProducer,
            IOrderRepository orderRepository,
            IOrderPolicy orderPolicy, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _messageProducer = messageProducer;
            _orderRepository = orderRepository;
            _orderPolicy = orderPolicy; 
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }
        public async Task<Order> CreateAsync(Order order)
        {
            var orderItemsDic = order.OrderItems.ToDictionary(aa => aa.ProductId, order => order);
            ProductDto[]? products = await GetProducts(order);

            var orderItems = new List<OrderItem>();
            foreach (var product in products)
            {
                var orderItem = orderItemsDic[product.Id];
                orderItem.UnitPrice = product.Price;
                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;


            order = _orderPolicy.Apply(order);

            order.CalculateTotal();

            await _orderRepository.InsertAsync(order);
            var message = order.OrderItems.Select(aa =>
            {
                var orderedProduct = _mapper.Map<OrderedProductDto>(aa);
                orderedProduct.Quantity += aa.FreeItemsQuatity;
                return orderedProduct;
            });
            _messageProducer.SendMessage(message);
            return order;
        }

        private async Task<ProductDto[]?> GetProducts(Order order)
        {
            var httpClient = _httpClientFactory.CreateClient("warehouse");

            var json = JsonConvert.SerializeObject(order.OrderItems.Select(x => x.ProductId));
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var a = await httpClient.PostAsync("products", content);
            var b = await a.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<ProductDto[]>(b);
            return products;
        }
    }
}
