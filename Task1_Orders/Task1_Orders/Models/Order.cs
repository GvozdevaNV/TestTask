using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Orders.Models
{
    class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }

        public Order()
        {
            Items = new List<OrderItem>();
        }
    }
}
