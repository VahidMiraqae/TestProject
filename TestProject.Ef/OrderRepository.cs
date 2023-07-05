using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core;

namespace TestProject.Ef
{
    public class OrderRepository : IOrderRepository
    {
        private WebDbContext _db;

        public OrderRepository(WebDbContext db)
        {
            _db = db;
        }
        public async Task InsertAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }
    }
}
