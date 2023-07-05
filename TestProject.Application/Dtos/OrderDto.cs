using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core;

namespace TestProject.Application.Dtos
{
    public class OrderDto
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public double Discount { get; set; }
        public decimal Total { get; private set; }
        public decimal DiscountedTotal { get; set; }
        public OrderItemDto[] OrderItems { get; set; }
    }
}
