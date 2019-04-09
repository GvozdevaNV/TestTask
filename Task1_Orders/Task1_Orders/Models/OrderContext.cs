using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Task1_Orders.Models
{
    class OrderContext : DbContext
    {
        static OrderContext()
        {
            Database.SetInitializer<OrderContext>(new OrderDBInitializer());
        }

        public OrderContext()
            : base("DefConnection")
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
