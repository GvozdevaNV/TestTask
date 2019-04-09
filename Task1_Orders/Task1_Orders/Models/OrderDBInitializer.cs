using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Task1_Orders.Models
{
    class OrderDBInitializer : DropCreateDatabaseAlways<OrderContext>
    {
        //public override void InitializeDatabase(OrderContext db)
        //{
        //    db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
        //                                  string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
        //                                  db.Database.Connection.Database));

        //    base.InitializeDatabase(db);
        //}

        protected override void Seed(OrderContext db)
        {
            OrderDBServices dbServices = new OrderDBServices();

            Order o1 = new Order();
            Order o2 = new Order();
            List<Order> orders = new List<Order> { o1, o2 };

            //OrderItem oi1 = new OrderItem { Title = "Кастрюля", Count = 1, Price = 650.7M, Order = o1};
            //OrderItem oi2 = new OrderItem { Title = "Контейнер", Count = 2, Price = 91.82M, Order = o1};
            //OrderItem oi3 = new OrderItem { Title = "Тарелка", Count = 10, Price = 109.48M, Order = o2};
            //OrderItem oi4 = new OrderItem { Title = "Сковородка", Count = 3, Price = 275.82M, Order = o1};
            //OrderItem oi5 = new OrderItem { Title = "Мантоварка", Count = 1, Price = 3292.68M, Order = o2 };

            //просто для теста делаю, чтобы amount, чтобы amount проще было отслеживать
            OrderItem oi1 = new OrderItem { Title = "Кастрюля", Count = 1, Price = 1.0M, Order = o1 };
            OrderItem oi2 = new OrderItem { Title = "Контейнер", Count = 1, Price = 1.0M, Order = o1 };
            OrderItem oi3 = new OrderItem { Title = "Тарелка", Count = 1, Price = 1.0M, Order = o2 };
            OrderItem oi4 = new OrderItem { Title = "Сковородка", Count = 1, Price = 1.0M, Order = o1 };
            OrderItem oi5 = new OrderItem { Title = "Мантоварка", Count = 1, Price = 1.0M, Order = o2 };
            List<OrderItem> orderItems = new List<OrderItem> { oi1, oi2, oi3, oi4, oi5 };

            orderItems = dbServices.CountOrderItemsAmount(db, orderItems);
            db.OrderItems.AddRange(orderItems);
            orders = dbServices.CountOrderAmountByItems(db, orders);
            db.Orders.AddRange(orders);
            db.SaveChanges();

            base.Seed(db);
        }
    }
}
