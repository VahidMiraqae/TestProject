using TestProject.Core;

namespace TestProject.Application
{
    public interface IOrderPolicy
    {
        Order Apply(Order order);
    }
}