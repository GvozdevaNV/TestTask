using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Orders.Models
{
    class OrderItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
