﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.IO;

namespace Big_Capital.Capital_Logic
{
    class PlayerInterface
    {
        String nickName;    
        List<CurOwned> wallet = new List<CurOwned>();
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
        StockExchange st = new StockExchange(startCur);
        TimerSender timer = new TimerSender(30000);

        //Конструкторы
        public PlayerInterface(String name, CurOwned own)
        {
            wallet.Add(own);
            this.nickName = name;
            timer.Elapsed += new ElapsedEventHandler(RandomTimerCallBack);
            timer.Sender = this;
            timer.Start();
        }
        public PlayerInterface(String name, List<CurOwned> own)
        {
            nickName = name;
            wallet = own;
            timer.Elapsed += new ElapsedEventHandler(RandomTimerCallBack);
            timer.Sender = this;
            timer.Start();
        }
        public static String ReadName()
        {
            Console.Write("Введите имя игрока: ");
            return Console.ReadLine();
        }
        public Boolean MenuExit
        {
            get;
            set;
        }
        //Timer callback
        private static void RandomTimerCallBack(object sender, Object stateInfo)
        {
            PlayerInterface player = (PlayerInterface)(sender as TimerSender).Sender;
            player.st.AddRandomOrders();
            player.st.RemoveRandomOrders(player);
            System.Diagnostics.Debug.WriteLine("Таймер отработал");
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
        public String GetCurOwned()
        {
            String output = "\n";
            foreach(CurOwned cur in wallet)
            {
                cur.Cost = st.GetQuotations().ToList().Find(x => x.GetName() == cur.GetName()).Cost;//Синхронизация цены со StockExchange
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
        public StockExchange GetStockExchange()     //получение экземпляра StockExchange
        {
            return st;
        }

        //player interact
        public void ShowMenu()
        {
            Menu.ShowMenu(new Menu[]
            {
                new Menu("Начать игру", delegate(PlayerInterface sender){ Console.Clear(); ShowGame(); }),
                new Menu("Настройки", delegate(PlayerInterface sender){ Console.WriteLine("Settings?"); }),
                new Menu("Выход", delegate(PlayerInterface sender){ sender.MenuExit = true; })
            }, this);
        }

        private void ShowGame()
        {
            Menu.ShowMenu(new Menu[]
            {
                new Menu("Показать котировки", 
                delegate(PlayerInterface sender){
                    Console.Clear();
                    sender.st.ShowQuotations();
                }),
                new Menu("Купить/Продать", delegate(PlayerInterface sender){
                    Console.Clear();
                    st.AddRandomOrders();//убрать рандом
                    //st.ShowOrders(startCur[0], startCur[5]);
                    st.RemoveRandomOrders(this);
                    st.Trade(this);
                }),
                new Menu("Обновить ордеры (Debug)",
                delegate(PlayerInterface sender){
                    st.AddRandomOrders();
                    st.RemoveRandomOrders(this);
                    Console.WriteLine("Ордеры обновленны!");
                }),
                new Menu("Личный кабинет", 
                delegate(PlayerInterface sender){
                    Console.Clear();
                    Console.WriteLine("Наименование:\t\tЦена:\tКоличество\n" + GetCurOwned());
                    Console.WriteLine("Ваши ордеры: ");
                    Console.WriteLine(st.ShowPlayerOrders());
                }),
                new Menu("Главное меню", delegate(PlayerInterface sender){ Console.Clear(); sender.MenuExit = true; })
            }, this);
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
        public delegate void MenuDelegate(PlayerInterface sender);
        public Menu(String menuName, MenuDelegate menuDelegate)
        {
            this.menuName = menuName;
            del = menuDelegate;
        }
        public static void ShowMenu(Menu[] menu, PlayerInterface sender)
        {
            while (!sender.MenuExit)
            {
                for (int i = 0; i < menu.Length; i++)
                {
                    Console.WriteLine("\n" + (i + 1) + ")" + menu[i].menuName);
                }
                try
                {
                    menu[Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1].del.DynamicInvoke(sender);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    break;
                }
            }
            sender.MenuExit = false;
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
