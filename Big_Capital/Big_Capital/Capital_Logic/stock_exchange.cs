using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class StockExchange
    {
        List<Order> orderSell = new List<Order>();
        List<Order> orderBuy = new List<Order>();
        Currency[] quotations;
        readonly static Int32 mainCurCount = 4;

        public StockExchange(Currency[] c)
        {
            this.quotations = c;
        }
        public void ShowQuotations()
        {
            Console.WriteLine("Валютные пары:\nНаименование:\t\t\tЦена:");
            for (int i = 0; i < quotations.Length; i++)
                Console.WriteLine(i + " " + quotations[i].GetCur());
        }
        public void Trade()
        {
            ShowQuotations();
            Console.Write("\tДля начала торгов введите номер 1 валюты и номер 2 валюты через пробел");
            string[] tokens = Console.ReadLine().Split();
            Int32 one = Convert.ToInt32(tokens[0]);
            Int32 two = Convert.ToInt32(tokens[1]);
            Console.WriteLine("\tВы выбрали валютную пару: " + quotations[one].GetName() + "/" + quotations[two], "\n\n\n");
        }
        public void AddRandomOrders()
        {
            Random rnd = new Random();
            for(int i = 0; i < mainCurCount; i++)
            {
                for(int j = 0; j < quotations.Length; j++)
                {
                    if(i != j)
                    {
                        //sell orders generation
                        Int32 rCount = rnd.Next(1, 10);

                        for(int k = 0; k < rCount; k++)
                        {
                            Currency main = quotations[i].Clone();
                            Currency second = quotations[j].Clone();
                            main.Cost += rnd.Next(-10, 10) / 100.0 * main.Cost;
                            second.Cost += rnd.Next(-10, 10) / 100.0 * second.Cost;
                            orderSell.Add(new Order(rnd.Next(100), main, second));
                        }

                        //buy orders generation
                        rCount = rnd.Next(1, 10);
                        for (int k = 0; k < rCount; k++)
                        {
                            Currency main = quotations[i].Clone();
                            Currency second = quotations[j].Clone();
                            main.Cost += rnd.Next(-10, 10) / 100.0 * main.Cost;
                            second.Cost += rnd.Next(-10, 10) / 100.0 * second.Cost;
                            orderBuy.Add(new Order(rnd.Next(100), main, second));
                        }
                    }
                }
            }
        }
        public void RemoveRandomOrders()
        {
            Random rnd = new Random();
            Int32 ordersToRemove = rnd.Next(orderSell.Count - 1);
            for(int i = 0; i < ordersToRemove; i++)
            {
                orderSell.RemoveAt(rnd.Next(orderSell.Count - 1));
            }
            ordersToRemove = rnd.Next(orderBuy.Count - 1);
            for (int i = 0; i < ordersToRemove; i++)
            {
                orderBuy.RemoveAt(rnd.Next(orderBuy.Count - 1));
            }
        }
        public void ShowOrders(Currency cur1, Currency cur2)
        {
            List<Order> sellPair = orderSell.FindAll(
                delegate(Order x) {
                    if (x.GetFirst().GetName() == cur1.GetName() && x.GetSecond().GetName() == cur2.GetName())
                    { return true; }
                    else return false;
                });
            List<Order> buyPair = orderBuy.FindAll(
                delegate (Order x) {
                    if (x.GetFirst().GetName() == cur1.GetName() && x.GetSecond().GetName() == cur2.GetName())
                    { return true; }
                    else return false;
                });

            sellPair.Sort(
                delegate (Order x, Order y) 
                { return (x.GetFirst().Cost / x.GetSecond().Cost).CompareTo(y.GetFirst().Cost / y.GetSecond().Cost); });
            buyPair.Sort(
                delegate (Order x, Order y)
                { return (x.GetFirst().Cost / x.GetSecond().Cost).CompareTo(y.GetFirst().Cost / y.GetSecond().Cost); });

            Console.WriteLine("Ордеры на продажу:\nЦена\t\t" + cur1.GetName() + "\t" + cur2.GetName());
            foreach(Order sell in sellPair)
            {
                Console.WriteLine(sell.GetOrder());
            }
            Console.WriteLine("Ордеры на покупку:\nЦена\t\t" + cur1.GetName() + "\t" + cur2.GetName());
            foreach (Order buy in buyPair)
            {
                Console.WriteLine(buy.GetOrder());
            }
        }
    }
    class Order
    {
        /*
         * возможность покупки валюты по ордеру
         * возможность продажи валюты по ордеру
         * при покупке валюты по ордеру: если куплено меньшее количество, чем указано в ордере
         * тогда вычесть из него количество купленой валюты
         * кошельки для валют
         * система рандомной покупки и продажи ордеров
         * запоминание игрока, кошелька, последних котировок и ордеров
         */
        Double count;
        Currency cur1;
        Currency cur2;
        public Order(Double count, Currency cur1, Currency cur2)
        {
            this.count = count;
            this.cur1 = cur1;
            this.cur2 = cur2;
        }
        public Order(Order order)
        {
            this.count = order.count;
            this.cur1 = cur1.Clone();
            this.cur2 = cur2.Clone();
        }
        public Order Clone()
        {
            return new Order(count, cur1.Clone(), cur2.Clone());// {count = this.count, cur1 = this.cur1.Clone(), cur2 = this.cur2.Clone()};
        }

        public String GetOrder()
        {
            return String.Format("{0:F6}\t{1}\t{2:F6}", GetFirst().Cost / GetSecond().Cost, GetCount(), GetFirst().Cost / GetSecond().Cost * GetCount());
        }
        public Currency GetFirst()
        {
            return cur1;
        }
        public Currency GetSecond()
        {
            return cur2;
        }
        public Double GetCount()
        {
            return count;
        }
    }
}
