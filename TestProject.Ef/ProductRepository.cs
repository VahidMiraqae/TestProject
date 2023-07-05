using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core;

namespace TestProject.Ef
{
    public class ProductRepository : IProductRepository
    {
        private WarehouseDbContext _db;

        public ProductRepository(WarehouseDbContext db)
        {
            _db = db;
        }

        public async Task<Product[]> GetAllAsync()
        {
            return await _db.Products.ToArrayAsync();
        }

        public async Task<Product[]> GetProductList(IEnumerable<int> ids)
        {
            var idsArray = ids.ToArray();
            return await _db.Products.Where(aa => idsArray.Contains(aa.Id))
                .ToArrayAsync();
        }

        public void UpdateProducts(Product[] products)
        {
            _db.UpdateRange(products);
            _db.SaveChanges();
        }
    }
}
