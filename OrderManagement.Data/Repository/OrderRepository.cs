using Dapper;
using Microsoft.Extensions.Configuration;
using OrderManagement.Data.DataUtilities;
using OrderManagement.Data.IRepository;
using OrderManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        //Declaration
        private IConfiguration _config;
        private readonly string _connectionString;
        //Constructor
        public OrderRepository(IConfiguration config) 
        {
            _config = config;
            _connectionString = _config.GetConnectionString("OrderManagementDB");
        }

        //Create DB Connection
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        #region Get Orders List
        public async Task<List<Order>> GetOrdersList() 
        {
            List<Order> orderList = new List<Order>();
            using (var dbConnection = CreateConnection())
            {
                orderList =  (await dbConnection.QueryAsync<Order>(SqlQueryConstants.GetOrdersList)).ToList();
            }
            return orderList;
        }
        #endregion

        #region Save Order
        public async Task<Order> SaveOrder(Order orderRequest)
        {
            Order orderResponse = new Order();
            orderResponse.Status = "FAILURE";
            if (orderRequest != null) 
            {
                using (var dbConnection = CreateConnection())
                {
                    orderResponse.OrderId = (await dbConnection.QueryAsync<long>(SqlQueryConstants.SaveOrder, orderRequest)).SingleOrDefault();
                    if (orderResponse.OrderId > 0) 
                    {
                        orderResponse.Status = "SUCCESS";
                    }
                }
            }
            return orderResponse;
        }

        #endregion

        #region Update Order
        public async Task<Order> UpdateOrder(Order orderRequest)
        {
            Order orderResponse = new Order();
            orderResponse.Status = "FAILURE";
            if (orderRequest != null && orderRequest.OrderId > 0)
            {
                using (var dbConnection = CreateConnection())
                {
                    await dbConnection.ExecuteAsync(SqlQueryConstants.UpdateOrder, orderRequest);
                    orderResponse.OrderId = orderRequest.OrderId;
                    orderResponse.Status = "SUCCESS";
                }
            }
            return orderResponse;
        }
        #endregion

        #region Get Order By Id
        public async Task<Order> GetOrderById(long orderId)
        {
            Order orderDetails = new Order();
            if (orderId > 0)
            {
                using (var dbConnection = CreateConnection())
                { 
                    orderDetails = (await dbConnection.QueryAsync<Order>(SqlQueryConstants.GetOrderById, new { OrderId = orderId })).SingleOrDefault();
                }
            }
            return orderDetails;
        }
        #endregion

        #region Delete Order By Id
        public async Task<DeleteOrder> DeleteOrderById(long[] orderIdList)
        {
            DeleteOrder deleteOrderResponse = new DeleteOrder();
            deleteOrderResponse.Status = "FAILURE";
            if (orderIdList != null && orderIdList.Length > 0)
            {
                using (var dbConnection = CreateConnection())
                {
                    foreach (var orderId in orderIdList)
                    {
                        if (orderId > 0)
                        {
                            await dbConnection.ExecuteAsync(SqlQueryConstants.DeleteOrderById, new { OrderId = orderId });
                            deleteOrderResponse.Status = "SUCCESS";
                        }
                    }
                }
            }
            return deleteOrderResponse;
        }
        #endregion
    }
}
