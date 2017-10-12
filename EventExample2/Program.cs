using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EventExample2
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Waitor waitor = new Waitor();
            customer.Order += waitor.Action;
            customer.Action();
            customer.PayTheBill();

        }
    }

    public class OrderEventArgs : EventArgs
    {
        public string DishName { get; set; }
        public string Size { get; set; }
        //
    }
   // public delegate void OrderEventHandler(Customer customer, OrderEventArgs e);

    public class Customer
    {
        //public event OrderEventHandler Order;  //自己申明一個事件
        public event EventHandler Order; //使用內建的EventHandler,直接表明要申明一個事件

        public double Bill { get; set; }
        public void PayTheBill()
        {
            Console.WriteLine("I will pay ${0}", this.Bill);
        }
        public void WalkIn()
        {
            Console.WriteLine("Walk into the restaurant");
        }
        public void SitDown()
        {
            Console.WriteLine("Sit down");
        }
        public void Think()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Let me think...");
                Thread.Sleep(1000);
            }
            //在Think裡面調用OnOrder這個方法,專門用於觸發事件請用On+事件名稱
            this.OnOrder("Kongpao Chicken", "large");
        }
        protected void OnOrder(string dishName, string size) 
        {
            if (this.Order != null)
            {
                OrderEventArgs e = new OrderEventArgs();
                e.DishName = dishName;
                e.Size = size;
                this.Order.Invoke(this, e);
            }
        }

        public void Action()
        {
            Console.ReadLine();
            this.WalkIn();
            this.SitDown();
            this.Think();
        }
    }

    public class Waitor
    {
        public void Action(object sender, EventArgs e)
        {
            Customer customer = sender as Customer; //類型轉換,傳進來的參數當作是customer class
            OrderEventArgs orderinfo = e as OrderEventArgs; //類型轉換,傳進來的參數當作是EventArgs class

            Console.WriteLine("I will serve you the dish - {0}", orderinfo.DishName);
            double price = 10;
            switch (orderinfo.Size)
            {
                case "small":
                    price = price * 0.5;
                    break;
                case "large":
                    price = price * 1.5;
                    break;
                default:
                    break;
            }
            customer.Bill += price;
        }
    }
}
