using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Data.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public long OrderNumber { get; set; }
        public decimal OrderAmount { get; set; }
        public long VendorId { get; set; }
        public string VendorName{ get; set; }
        public int Shop { get; set; }
        public string Status { get; set; }
    }
}
