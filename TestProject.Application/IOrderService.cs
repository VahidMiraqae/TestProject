using TestProject.Core;

namespace TestProject.Application
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(Order order);
    }
}