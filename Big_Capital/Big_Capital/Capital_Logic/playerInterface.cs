using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.IO;

namespace Big_Capital.Capital_Logic
{
    sealed class PlayerInterface
    {
        private String nickName;
        private List<CurOwned> wallet = new List<CurOwned>();
        public readonly static Currency[] startCur = {
            new Currency("USD", 1),
            new Currency("BTC", 10889.00000001),
            new Currency("ETH", 1183.32000003),
            new Currency("RUR", 0.01778),
            new Currency("DASH", 743.89857603),
            new Currency("LIZA", 0.67990000),
            new Currency("BCC", 1609.63799991),
            new Currency("DOGE", 0.00639700)
        };//стартовая валюта и её стоимость
        private static volatile PlayerInterface instance; //singleton
        private static object syncRoot = new Object();
        //private StockExchange StockExchange.Instance = new StockExchange(startCur); //remove, when ST is singleton
        private TimerSender timer = new TimerSender(30000);

        //Конструкторы
        public static PlayerInterface Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PlayerInterface();
                    }
                }
                return instance;
            }
        }
        public PlayerInterface()
        {
            StockExchange.Instance.SetQuotations(startCur);
            this.nickName = "";
            timer.Elapsed += new ElapsedEventHandler(RandomTimerCallBack);
            timer.Sender = this;
            timer.Start();
        }
        public void ReadName()
        {
            Console.Write("Введите имя игрока: ");
            nickName = Console.ReadLine();
        }
        public Boolean MenuExit
        {
            get;
            set;
        }
        //Timer callback
        private static void RandomTimerCallBack(object sender, Object stateInfo)
        {
            StockExchange.Instance.AddRandomOrders();
            StockExchange.Instance.RemoveRandomOrders();
            System.Diagnostics.Debug.WriteLine("Таймер отработал"); //debug
        }

        //Взаимодействие с кошельком
        public void AddCurOwned(CurOwned c)
        {
            if(wallet.Exists(x => x.GetName() == c.GetName()))
            {
                wallet.Find(x => x.GetName() == c.GetName()).Owned += c.Owned;
            }
            else
                wallet.Add(c);
        }
        private String GetCurOwned()
        {
            String output = "\n";
            foreach(CurOwned cur in wallet)
            {
                cur.Cost = StockExchange.Instance.GetQuotations().ToList().Find(x => x.GetName() == cur.GetName()).Cost;//Синхронизация цены со StockExchange
                output += cur.GetCur();
                output += "\n";
            }
            if(output == "")
                output = "Пусто";
            return output;
        }
        public void RemoveCurOwned(CurOwned c)
        {
            if (wallet.Exists(x => x.GetName() == c.GetName()))
            {
                if (wallet.Find(x => x.GetName().Equals(c.GetName())).Owned > c.Owned)
                    wallet.Find(x => x.GetName().Equals(c.GetName())).Owned -= c.Owned;
                else
                    wallet.Remove(wallet.Find(x => x.GetName().Equals(c.GetName())));
            }
        }
        public Boolean IsCurOwned(CurOwned cur)
        {
            return wallet.Exists(x => x.GetName() == cur.GetName() && x.Owned >= cur.Owned);
        }

        //player interact
        public void ShowMenu()
        {
            Menu.ShowMenu(new Menu[]
            {
                new Menu("Начать игру", delegate(){ Console.Clear(); ShowGame(); }),
                new Menu("Настройки", delegate(){ Console.WriteLine("Settings?"); }),
                new Menu("Выход", delegate(){ PlayerInterface.Instance.MenuExit = true; })
            });
        }

        private void ShowGame()
        {
            Menu.ShowMenu(new Menu[]
            {
                new Menu("Показать котировки", 
                delegate(){
                    Console.Clear();
                    StockExchange.Instance.ShowQuotations();
                }),
                new Menu("Купить/Продать", delegate(){
                    Console.Clear();
                    StockExchange.Instance.AddRandomOrders();//убрать рандом
                    //StockExchange.Instance.ShowOrders(startCur[0], startCur[5]);
                    StockExchange.Instance.RemoveRandomOrders();
                    StockExchange.Instance.Trade();
                }),
                new Menu("Обновить ордеры (Debug)",
                delegate(){
                    StockExchange.Instance.AddRandomOrders();
                    StockExchange.Instance.RemoveRandomOrders();
                    Console.WriteLine("Ордеры обновленны!");
                }),
                new Menu("Личный кабинет", 
                delegate(){
                    Console.Clear();
                    Console.WriteLine("Наименование:\t\tЦена:\tКоличество\n" + GetCurOwned());
                    Console.WriteLine("Ваши ордеры: ");
                    Console.WriteLine(StockExchange.Instance.ShowPlayerOrders());
                }),
                new Menu("Главное меню", delegate(){ Console.Clear(); PlayerInterface.Instance.MenuExit = true; })
            }, "Добро пожаловать, " + nickName + "!");
        }

        //Сохранение/Загрузка
        //public static void Save(String filePath, PlayerInterface playerInterface)
        //{
        //    FileStream outFile = File.Create(filePath);
        //    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(playerInterface.GetType());
        //    serializer.Serialize(outFile, playerInterface);
        //}
    }

    class Menu
    {
        private String menuName;
        private Delegate del;
        public delegate void MenuDelegate();
        public Menu(String menuName, MenuDelegate menuDelegate)
        {
            this.menuName = menuName;
            del = menuDelegate;
        }
        public static void ShowMenu(Menu[] menu, String header = "")
        {
            while (!PlayerInterface.Instance.MenuExit)
            {
                if(header != "")
                {
                    Console.WriteLine(header);
                }
                for (int i = 0; i < menu.Length; i++)
                {
                    Console.WriteLine("\n" + (i + 1) + ")" + menu[i].menuName);
                }
                try
                {
                    menu[Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1].del.DynamicInvoke();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    break;
                }
            }
            PlayerInterface.Instance.MenuExit = false;
        }
        
    }

    class TimerSender : Timer
    {
        public TimerSender(double interval) : base(interval) { }
        public object Sender
        {
            get;
            set;
        }
    }
}
