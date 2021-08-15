using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Data.DataUtilities
{
    class SqlQueryConstants
    {
        public const string GetOrdersList = "SELECT OrderId,OrderNumber,OrderDate,OrderAmount, VendorName, Shop,CreatedTime,UpdateTime FROM tbl_Orders ORDER By CreatedTime ASC";

        public const string SaveOrder = "INSERT INTO tbl_Orders (OrderNumber,OrderDate, OrderAmount, VendorName, Shop) VALUES(@OrderNumber,@OrderDate, @OrderAmount, @VendorName, @Shop) SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

        public const string UpdateOrder = "UPDATE tbl_Orders SET OrderNumber = @OrderNumber, OrderDate = @OrderDate, OrderAmount = @OrderAmount, VendorName = @VendorName, Shop = @Shop Where OrderId = @OrderId";

        public const string GetOrderById = "SELECT OrderId,OrderNumber,OrderDate,OrderAmount, VendorName, Shop,CreatedTime,UpdateTime FROM tbl_Orders Where OrderId = @OrderId";

        public const string DeleteOrderById = "DELETE FROM tbl_Orders Where OrderId = @OrderId";
    }
}
