using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core;

namespace TestProject.Application
{
    public class OrderPolicy : IOrderPolicy
    {
        public Order Apply(Order order)
        {
            int discount = 0;
            foreach (var orderItem in order.OrderItems)
            {
                var dis = ((orderItem.Quantity - 5) / 3) * 2;
                discount += dis > 0 ? dis : 0;

                orderItem.FreeItemsQuatity = (orderItem.Quantity / 10);
            }
            order.Discount = discount / 100.0;
            return order;
        }
    }
}
