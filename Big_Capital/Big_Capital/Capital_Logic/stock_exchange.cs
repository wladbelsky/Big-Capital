using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class stock_exchange
    {
        Order[] order_sell;
        Order[] order_buy;
        Currency[] quotations;
        public stock_exchange(Currency[] c)
        {
            this.quotations = c;
        }
        public void Show()
        {
            Console.Write("\n\tВалютные пары:\n\tНаименование:\t\t\tЦена:");
            for (int i = 0; i < quotations.Length; i++)
                Console.Write(i + " " + quotations[i].GetCur());
        }
        public void Trade()
        {
            Show();
            Console.Write("\tДля начала торгов введите номер 1 валюты и номер 2 валюты через пробел");
            string[] tokens = Console.ReadLine().Split();
            Int32 one = Convert.ToInt32(tokens[0]);
            Int32 two = Convert.ToInt32(tokens[1]);
            Console.WriteLine("\tВы выбрали валютную пару: " + quotations[one].GetName() + "/" + quotations[two], "\n\n\n");
        }
        public void ShowOrders(Currency cur1, Currency cur2)
        {
            Array.Sort(order_sell,
            delegate(Order x, Order y) { return x.GetFirst().Cost.CompareTo(y.GetFirst().Cost); }); 
            Array.Sort(order_buy,
            delegate(Order x, Order y) { return y.GetFirst().Cost.CompareTo(x.GetFirst().Cost); }); 
            foreach(Order sell in order_sell)
            {
                Console.WriteLine();
            }
        }
    }
    class Order
    {
        Double count;
        Currency cur1;
        Currency cur2;
        public Order(Double count, Currency cur1, Currency cur2)
        {
            this.cur1 = cur1;
            this.count = count;
            this.cur2 = cur2;
        }
        public Currency GetFirst()
        {
            return cur1;
        }
        public Currency GetSecond()
        {
            return cur2;
        }
    }     
}
