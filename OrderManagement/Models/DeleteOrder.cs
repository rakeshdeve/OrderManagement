using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Models
{
    public class DeleteOrder
    {
        public long[] OrderIds { get; set; }
        public string Status { get; set; }
    }
}
