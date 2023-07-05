using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core
{
    public interface IOrderRepository
    {
        Task InsertAsync(Order order);
    }
}
