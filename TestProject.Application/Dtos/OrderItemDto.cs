using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core;

namespace TestProject.Application.Dtos
{
    public class OrderItemDto
    { 
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int FreeItemsQuatity { get; set; }
        public decimal UnitPrice { get; set; } 
    }
}
