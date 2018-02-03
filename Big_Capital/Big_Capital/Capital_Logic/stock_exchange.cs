using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    sealed class StockExchange
    {
        private List<Order> orderSell = new List<Order>();
        private List<Order> orderBuy = new List<Order>();
        private Currency[] quotations;
        private readonly static Int32 mainCurCount = 4;
        private static volatile StockExchange instance; //singleton
        private static object syncRoot = new Object();

        //Конструкторы
        public static StockExchange Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new StockExchange();
                    }
                }
                return instance;
            }
        }

        //Котировки
        public void ShowQuotations()
        {
            Console.WriteLine("Котировки:\nНаименование:\t\t\tЦена:");
            for (int i = 0; i < quotations.Length; i++)
            {
                if (i == mainCurCount)
                {
                    Console.SetCursorPosition(50, Console.CursorTop - mainCurCount - 1);
                    Console.WriteLine("Наименование:\t\t\tЦена:");
                }
                if (i >= mainCurCount)
                    Console.SetCursorPosition(50, Console.CursorTop);
                Console.WriteLine(i + " " + quotations[i].GetCur());
            }
        }
        public Currency[] GetQuotations()
        {
            return quotations;
        }
        public void SetQuotations(Currency[] quotations)
        {
            this.quotations = quotations;
        }

        //Торги
        public void Trade()
        {
            Console.WriteLine("Для начала торгов введите номер 1 валюты и номер 2 валюты через пробел");//покупаем первую за вторую)
            ShowQuotations();
            string[] tokens = Console.ReadLine().Split();
            Int32 one = Convert.ToInt32(tokens[0]);
            Int32 two = Convert.ToInt32(tokens[1]);
            Console.WriteLine("\tВы выбрали валютную пару: " + quotations[one].GetName() + "/" + quotations[two].GetName());
            ShowOrders(quotations[one], quotations[two]);
            Menu.ShowMenu(new Menu[]
            {
                new Menu("Купить", 
                delegate(){
                    Console.Write("Выберите желаемое количество для покупки: ");
                    Double count = Convert.ToDouble(Console.ReadLine());
                    Order order = new Order(count, quotations[one], quotations[two], true);
                    if(PlayerInterface.Instance.IsCurOwned(new CurOwned(quotations[two], count * quotations[one].Cost / quotations[two].Cost)))
                    {
                        AddBuyOrder(order);
                        Console.WriteLine("Ордер успешно выставлен на покупку!");
                    }
                    else
                        Console.WriteLine("Недостаточно средств!");

                    PlayerInterface.Instance.MenuExit = true;
                }),
                new Menu("Продать", 
                delegate(){
                    Console.WriteLine("Введите количество для продажи: ");
                    Double count = Convert.ToDouble(Console.ReadLine());
                    Order order = new Order(count, quotations[one], quotations[two], true);
                    if(PlayerInterface.Instance.IsCurOwned(new CurOwned(quotations[one], count)))
                    {
                        AddSellOrder(order);//первую за вторую!!! xD
                        Console.WriteLine("Ордер успешно выставлен на продажу!");
                    }
                    else
                        Console.WriteLine("Недостаточно средств!");
                    PlayerInterface.Instance.MenuExit = true;
                }),
                new Menu("Назад", delegate(){ PlayerInterface.Instance.MenuExit = true; })
            });

        }
        public void AddBuyOrder(Order order)//добавить проверку на наличие валюты у игрока!!!
        {
            List<Order> pair = GetPair(orderSell, order.GetFirst(), order.GetSecond());
            Order buyOrder = pair.Find(x => x.GetCount() >= order.GetCount());
            if (buyOrder != null)
            {
                if (buyOrder.GetCount() == order.GetCount())
                {
                    orderSell.Remove(buyOrder);
                }
                else
                {
                    orderSell[orderSell.IndexOf(buyOrder)].SetCount(orderSell[orderSell.IndexOf(buyOrder)].GetCount() - buyOrder.GetCount());
                }
                PlayerInterface.Instance.AddCurOwned(new CurOwned(buyOrder.GetFirst(), order.GetCount()));

            }
            else
                orderBuy.Add(order);
            PlayerInterface.Instance.RemoveCurOwned(new CurOwned(order.GetSecond(), order.GetFirst().Cost / order.GetSecond().Cost * order.GetCount()));
        }
        public void AddSellOrder(Order order)
        {
            orderSell.Add(order);
            PlayerInterface.Instance.RemoveCurOwned(new CurOwned(order.GetFirst(), order.GetCount()));
        }

        //Случайные торги
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
                            orderSell.Add(new Order(rnd.Next(1, 100), main, second));
                            orderSell.Add(new Order(rnd.Next(1, 100), second, main));
                        }

                        //buy orders generation
                        rCount = rnd.Next(1, 10);
                        for (int k = 0; k < rCount; k++)
                        {
                            Currency main = quotations[i].Clone();
                            Currency second = quotations[j].Clone();
                            main.Cost += rnd.Next(-10, 10) / 100.0 * main.Cost;
                            second.Cost += rnd.Next(-10, 10) / 100.0 * second.Cost;
                            orderBuy.Add(new Order(rnd.Next(1, 100), main, second));
                            orderBuy.Add(new Order(rnd.Next(1, 100), second, main));
                        }
                    }
                }
            }
        }
        public void RemoveRandomOrders()
        {
            Random rnd = new Random();
            List<Order> newOrderSell = new List<Order>();
            List<Order> newOrderBuy = new List<Order>();
            for (int i = 0; i < quotations.Length; i++)//mainCurCount
            {
                for (int j = 0; j < quotations.Length; j++)
                {
                    if (i != j)
                    {
                        List<Order> buyPair = GetPair(orderBuy, quotations[i], quotations[j]);
                        List<Order> sellPair = GetPair(orderSell, quotations[i], quotations[j]);
                        if (buyPair.Count > 1 && sellPair.Count > 1)
                        {
                            Int32 soldOrdersCount = rnd.Next(1, sellPair.Count);
                            List<Order> soldPlayerOrders = sellPair.FindAll(x => x.IsPlayer == true);
                            if(soldPlayerOrders.Count > 0)
                            {
                                for(int k = 0; k < soldPlayerOrders.Count; k++)
                                {
                                    System.Diagnostics.Debug.WriteLine(soldPlayerOrders[k].GetOrder());//debug
                                    Console.WriteLine("Ваш ордер куплен {0}/{1}", soldPlayerOrders[k].GetFirst().GetName(), soldPlayerOrders[k].GetSecond().GetName());
                                    //sender.RemoveCurOwned(new CurOwned(soldPlayerOrders[k].GetFirst(), soldPlayerOrders[k].GetCount()));
                                    PlayerInterface.Instance.AddCurOwned(new CurOwned(soldPlayerOrders[k].GetSecond(), soldPlayerOrders[k].GetFirst().Cost / soldPlayerOrders[k].GetSecond().Cost * soldPlayerOrders[k].GetCount()));
                                    sellPair.Remove(soldPlayerOrders[k]);
                                }
                            }
                            sellPair.RemoveRange(0, soldOrdersCount);

                            Int32 boughtOrdersCount = rnd.Next(1, buyPair.Count);
                            List<Order> boughtPlayerOrders = buyPair.FindAll(x => x.IsPlayer == true);
                            if (rnd.Next(0, 100) <= 60)    //рост/падение
                            {
                                buyPair.Reverse();
                            }
                            if (boughtPlayerOrders.Count > 0)
                            {
                                for (int k = 0; k < boughtPlayerOrders.Count; k++)
                                {
                                    Console.WriteLine("Ваш ордер продан {0}/{1}", boughtPlayerOrders[k].GetFirst().GetName(), boughtPlayerOrders[k].GetSecond().GetName());
                                    PlayerInterface.Instance.AddCurOwned(new CurOwned(boughtPlayerOrders[k].GetFirst(), boughtPlayerOrders[k].GetCount()));
                                    //sender.RemoveCurOwned(new CurOwned(boughtPlayerOrders[k].GetSecond(), boughtPlayerOrders[k].GetFirst().Cost / boughtPlayerOrders[k].GetSecond().Cost * boughtPlayerOrders[k].GetCount()));
                                    buyPair.Remove(boughtPlayerOrders[k]);
                                }
                            }//Нужна проверка!
                            buyPair.RemoveRange(0, boughtOrdersCount);

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

        //Ордера
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
        public String ShowPlayerOrders()
        {
            String str = "";
            List<Order> playerSell = orderSell.FindAll(x => x.IsPlayer == true);
            List<Order> playerBuy = orderBuy.FindAll(x => x.IsPlayer == true);
            Console.WriteLine("Ордеры на продажу:\n");
            foreach(Order sell in playerSell)
            {
                str += "Цена\t\t" + sell.GetFirst().GetName() + "\t" + sell.GetSecond().GetName() + "\n" + sell.GetOrder();
            }
            Console.WriteLine("Ордеры на покупку:\n");
            foreach (Order buy in playerBuy)
            {
                str += "Цена\t\t" + buy.GetFirst().GetName() + "\t" + buy.GetSecond().GetName() + "\n" + buy.GetOrder();
            }
            return str;
        }
    }
    class Order
    {
        /*
         * возможность покупки валюты по ордеру                                                 check!
         * возможность продажи валюты по ордеру                                                 check!
         * при покупке валюты по ордеру: если куплено меньшее количество, чем указано в ордере  check!
         * тогда вычесть из него количество купленой валюты
         * кошельки для валют                                                                   check!
         * система рандомной покупки и продажи ордеров                                          check!
         * запоминание игрока, кошелька, последних котировок и ордеров                          not started yet
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
        public override bool Equals(object obj)
        {
            if (!(obj is Order)) return false;
            else
            {
                Order order = (Order) obj;
                return order.count == count && order.cur1.Equals(cur1) && order.cur2.Equals(cur2);
            }
        }
        public override int GetHashCode()
        {
            return count.GetHashCode() + cur1.GetHashCode() + cur2.GetHashCode();//base.GetHashCode();
        }
        public Boolean IsPlayer
        {
            get;
            set;
        }
        public String GetOrder()
        {
            return String.Format("{0:F8}\t{1}\t{2:F8}", GetFirst().Cost / GetSecond().Cost, GetCount(), GetFirst().Cost / GetSecond().Cost * GetCount());
        }
        public Currency GetFirst()
        {
            return cur1;
        }
        public Currency GetSecond()
        {
            return cur2;
        }
        public double GetCount()
        {
            return count;
        }
        public void SetCount(double value)
        {
            if (value >= 0)
                count = value;
            else
                count = 0;
        }
    }
}
