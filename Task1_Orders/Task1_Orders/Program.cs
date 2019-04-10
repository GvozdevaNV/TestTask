using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.Entity;
using Task1_Orders.Models;
using System.Threading.Tasks;

namespace Task1_Orders
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintOrderItemsByOrder();

            Task task1 = Task.Factory.StartNew(UpdateOrderPrimary);
            Task task2 = Task.Factory.StartNew(UpdateOrderSecondary);
            Task.WaitAll(task1, task2);
            PrintOrderItemsByOrder();

            Console.Read();
        }

        private static void UpdateOrderPrimary()
        {
            using (OrderContext db = new OrderContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
                {
                    try
                    {
                        Thread.CurrentThread.Name = "Primary";
                        Console.WriteLine("Запущен поток {0}\n", Thread.CurrentThread.Name);
                        var order = db.Orders.Include(x => x.Items).Single(o => o.Id == 1);
                        new OrderDBServices().InsertOrderItem(db, new OrderItem { Title = "Primary", Count = 1, Price = 1.0M, Order = order });
                        db.SaveChanges();
                        Console.WriteLine("Поток {0} завершил работу\n", Thread.CurrentThread.Name);
                        #region test
                        //PrintOrderItems(db);
                        //PrintOrderItemsByOrder(db);
                        //Console.WriteLine(new string('-', 50));

                        //int newCount;
                        //decimal newPrice;
                        //var orderItem = db.OrderItems.Single(oi => oi.Id == 6);
                        //newCount = 3;
                        //dbServices.UpdateOrderItemCountOrPrice(db, orderItem, newCount, null);
                        //newCount = 1;
                        //dbServices.UpdateOrderItemCountOrPrice(db, orderItem, newCount, null);
                        //newPrice = 100.0M;
                        //dbServices.UpdateOrderItemCountOrPrice(db, orderItem, null, newPrice);
                        //newPrice = 50.0M;
                        //dbServices.UpdateOrderItemCountOrPrice(db, orderItem, null, newPrice);
                        //newCount = 1;
                        //newPrice = 45.5M;
                        //dbServices.UpdateOrderItemCountOrPrice(db, orderItem, newCount, newPrice);
                        //dbServices.UpdateOrderItemCountOrPrice(db, orderItem, null, null);
                        //dbServices.DeleteOrderItem(db, orderItem);
                        //db.SaveChanges();

                        //PrintOrderItemsByOrder();
                        //Console.WriteLine(new string('-', 50));
                        #endregion
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        private static void UpdateOrderSecondary()
        {
            using (OrderContext db = new OrderContext())
            {
                //using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
                //{
                //    try
                //    {
                Thread.CurrentThread.Name = "Secondary";
                Console.WriteLine("Запущен поток {0}\n", Thread.CurrentThread.Name);
                var order = db.Orders.Include(x => x.Items).Single(o => o.Id == 1);
                new OrderDBServices().InsertOrderItem(db, new OrderItem { Title = "Secondary", Count = 1, Price = 1.0M, Order = order });
                db.SaveChanges();
                Console.WriteLine("Поток {0} завершил работу\n", Thread.CurrentThread.Name);
                //        tran.Commit();
                //    }
                //    catch(Exception ex)
                //    {
                //        tran.Rollback();
                //    }
                //}           
            }
        }

        private static void PrintOrderItems()
        {
            //todo: убрать orderId
            using (OrderContext db = new OrderContext())
            {
                var orderItems = new OrderDBServices().GetOrderItems(db);
                foreach (OrderItem oi in orderItems)
                {
                    Console.WriteLine("Id: {0}, Title: {1}, Count: {2}, Price: {3} Amount: {4}, OrderId: {5}", oi.Id, oi.Title, oi.Count, oi.Price, oi.Amount, oi.OrderId);
                }
                Console.WriteLine();
            }
        }

        private static void PrintOrders()
        {
            using (OrderContext db = new OrderContext())
            {
                var orders = new OrderDBServices().GetOrderItemsByOrder(db);
                foreach (Order o in orders)
                {
                    Console.Write("Заказ №" + o.Id + "\t");
                    Console.WriteLine("Сумма по итемам: " + o.Items.Count);
                    Console.WriteLine("Total amount: " + o.Amount);
                }
            }
            Console.WriteLine();
        }

        private static void PrintOrderItemsByOrder()
        {
            using (OrderContext db = new OrderContext())
            {
                var orders = new OrderDBServices().GetOrderItemsByOrder(db);
                foreach (Order o in orders)
                {
                    Console.WriteLine("Заказ №" + o.Id + "\n");
                    Console.WriteLine("Сумма по итемам: " + o.Items.Count);
                    Console.WriteLine("Total amount: " + o.Amount);
                    foreach (OrderItem oi in o.Items)
                    {
                        Console.WriteLine("Id: {0} Title: {1} Count: {2} Price: {3} Amount: {4}", oi.Id, oi.Title, oi.Count, oi.Price, oi.Amount);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
