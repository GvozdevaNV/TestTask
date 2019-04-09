using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Task1_Orders.Models
{
    class OrderDBServices
    {
        public List<OrderItem> CountOrderItemsAmount(OrderContext db, List<OrderItem> orderItems)
        {
            foreach (OrderItem oi in orderItems)
            {
                oi.Amount = oi.Price * oi.Count;
            }
            return orderItems;
        }

        public List<Order> CountOrderAmountByItems(OrderContext db, List<Order> orders)
        {
            foreach (Order o in orders)
            {
                o.Amount = o.Items.Sum(i => i.Amount);
            }
            return orders;
        }

        public List<OrderItem> GetOrderItems(OrderContext db)
        {
            var orderItems = db.OrderItems.ToList();
            return orderItems;
        }

        public List<Order> GetOrders(OrderContext db)
        {
            var orders = db.Orders.ToList();
            return orders;
        }

        public List<Order> GetOrderItemsByOrder(OrderContext db)
        {
            var orders = db.Orders.Include(x => x.Items).ToList();
            return orders;
        }

        public void InsertOrderItem(OrderContext db, OrderItem orderItem)
        {
            var orderItems = CountOrderItemsAmount(db, new List<OrderItem> { orderItem });
            db.OrderItems.AddRange(orderItems);

            UpdateOrderAmount(db, orderItem, orderItem.Amount);
        }

        public void UpdateOrderItemCountOrPrice(OrderContext db, OrderItem orderItem, int? newCount, decimal? newPrice)
        {
            decimal newAmount, oldAmount, price, divAmount;
            int count;
            var oi = db.OrderItems.Find(orderItem.Id);
            oldAmount = oi.Amount;
            if (newCount != null && newPrice == null)
            {
                price = oi.Price;
                oi.Count = (int)newCount;
                newAmount = CountOrderItemAmount(db, oi, (int)newCount, price);
            }
            else if(newCount == null && newPrice != null)
            {
                count = oi.Count;
                oi.Price = (decimal)newPrice;
                newAmount = CountOrderItemAmount(db, oi, count, (decimal)newPrice);
            }
            else if(newCount != null && newPrice != null)
            {
                oi.Price = (decimal)newPrice;
                oi.Count = (int)newCount;
                newAmount = CountOrderItemAmount(db, oi, (int)newCount, (decimal)newPrice);
            }
            else
            {
                return;
            }

            divAmount = newAmount - oldAmount;
            UpdateOrderAmount(db, orderItem, divAmount);
        }

        public void DeleteOrderItem(OrderContext db, OrderItem orderItem)
        {
            decimal oldAmount, divAmount;
            var oi = db.OrderItems.Find(orderItem.Id);
            oldAmount = oi.Amount;
            divAmount = -oldAmount;
            db.OrderItems.Remove(orderItem);

            UpdateOrderAmount(db, orderItem, divAmount);
        }

        private decimal CountOrderItemAmount(OrderContext db, OrderItem orderItem, int count, decimal price)
        {
            return db.OrderItems.Find(orderItem.Id).Amount = count * price;
        }

        private void UpdateOrderAmount(OrderContext db, OrderItem orderItem, decimal divAmount)
        {
            var order = db.Orders.Find(orderItem.OrderId);
            order.Amount += divAmount;
        }
    }
}
