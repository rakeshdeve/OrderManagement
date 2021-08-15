using OrderManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Data.IRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersList();
        Task<Order> SaveOrder(Order orderRequest);
        Task<Order> UpdateOrder(Order orderRequest);
        Task<Order> GetOrderById(long orderId);
        Task<DeleteOrder> DeleteOrderById(long[] orderIdList);

    }
}
