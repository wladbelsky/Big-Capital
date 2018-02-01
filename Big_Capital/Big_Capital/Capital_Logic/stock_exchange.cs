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
        public Currency[] GetQuotations()
        {
            return quotations;
        }
        public void Trade()//legacy
        {
            ShowQuotations();
            Console.Write("\tДля начала торгов введите номер 1 валюты и номер 2 валюты через пробел");
            string[] tokens = Console.ReadLine().Split();
            Int32 one = Convert.ToInt32(tokens[0]);
            Int32 two = Convert.ToInt32(tokens[1]);
            Console.WriteLine("\tВы выбрали валютную пару: " + quotations[one].GetName() + "/" + quotations[two], "\n\n\n");
        }
        public void BuyOrder(Order order, PlayerInterface sender)
        {
            List<Order> pair = GetPair(orderSell, order.GetFirst(), order.GetSecond());
            //Order buyOrder = pair.Find()
        }
        public void AddRandomOrders()
        {
            Random rnd = new Random();
            for (int i = 0; i < mainCurCount; i++)
            {
                for (int j = 0; j < quotations.Length; j++)
                {
                    if (i != j)
                    {
                        //sell orders generation
                        Int32 rCount = rnd.Next(1, 10);

                        for (int k = 0; k < rCount; k++)
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
        public void RemoveRandomOrders(PlayerInterface sender)
        {
            Random rnd = new Random();
            List<Order> newOrderSell = new List<Order>();
            List<Order> newOrderBuy = new List<Order>();
            for (int i = 0; i < mainCurCount; i++)
            {
                for (int j = 0; j < quotations.Length; j++)
                {
                    if (i != j)
                    {
                        List<Order> buyPair = StockExchange.GetPair(orderBuy, quotations[i], quotations[j]);
                        List<Order> sellPair = StockExchange.GetPair(orderSell, quotations[i], quotations[j]);
                        if (buyPair.Count > 1 && sellPair.Count > 1)
                        {
                            Int32 soldOrdersCount = rnd.Next(1, sellPair.Count - 1);
                            List<Order> soldPlayerOrders = sellPair.FindAll(x => x.IsPlayer == true);
                            if(soldPlayerOrders.Count > 0)
                            {
                                for(int k = 0; k < soldOrdersCount; k++)
                                {
                                    sender.AddCurOwned(new CurOwned(soldPlayerOrders[i].GetSecond(), soldPlayerOrders[i].GetCount()));
                                }
                            }
                            sellPair.RemoveRange(0, soldOrdersCount);

                            if (rnd.Next(0, 100) <= 60)    //рост/падение
                            {
                                buyPair.Reverse();
                            }
                            buyPair.RemoveRange(0, rnd.Next(1, buyPair.Count - 1));

                            newOrderSell.AddRange(sellPair);
                            newOrderBuy.AddRange(buyPair);
                            quotations[j].Cost = buyPair[0].GetSecond().Cost;
                        }
                    }
                }
            }
            orderBuy = newOrderBuy;
            orderSell = newOrderSell;
        }
        public static List<Order> GetPair(List<Order> list, Currency cur1, Currency cur2)
        {
            List<Order> pair = list.FindAll(
                delegate (Order x) {
                    if (x.GetFirst().GetName() == cur1.GetName() && x.GetSecond().GetName() == cur2.GetName())
                    { return true; }
                    else return false;
                });
            pair.Sort(
                delegate (Order x, Order y)
                { return (x.GetFirst().Cost / x.GetSecond().Cost).CompareTo(y.GetFirst().Cost / y.GetSecond().Cost); });
            return pair;
        }
        public void ShowOrders(Currency cur1, Currency cur2)
        {
            List<Order> sellPair = StockExchange.GetPair(orderSell, cur1, cur2);
            List<Order> buyPair = StockExchange.GetPair(orderBuy, cur1, cur2);
            buyPair.Reverse();

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
        public Order(Double count, Currency cur1, Currency cur2, Boolean isPlayer = false)
        {
            this.count = count;
            this.cur1 = cur1;
            this.cur2 = cur2;
            IsPlayer = isPlayer;
        }
        public Order(Order order)
        {
            this.count = order.count;
            this.cur1 = order.cur1.Clone();
            this.cur2 = order.cur2.Clone();
            this.IsPlayer = order.IsPlayer;
        }
        public Order Clone()
        {
            return new Order(count, cur1.Clone(), cur2.Clone());// {count = this.count, cur1 = this.cur1.Clone(), cur2 = this.cur2.Clone()};
        }
        public Boolean IsPlayer
        {
            get;
            set;
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
