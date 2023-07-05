using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core
{
    public interface IProductRepository
    {
        Task<Product[]> GetAllAsync();
        Task<Product[]> GetProductList(IEnumerable<int> enumerable);
        void UpdateProducts(Product[] products);
    }
}
