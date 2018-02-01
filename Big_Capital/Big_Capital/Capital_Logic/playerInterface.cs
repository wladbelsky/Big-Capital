﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class PlayerInterface
    {
        String nickName;    
        List<CurOwned> wallet = new List<CurOwned>();
        readonly static Currency[] startCur = {
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

        public PlayerInterface() => ReadName();
        public PlayerInterface(String name, CurOwned own)
        {
            wallet.Add(own);
            this.nickName = name;
        }
        public PlayerInterface(String name, List<CurOwned> own)
        {
            this.nickName = name;
            wallet = own;
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

        //wallet interact
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
            String output = "";
            foreach(CurOwned cur in wallet)
            {
                cur.Cost = st.GetQuotations().ToList().Find(x => x.GetName() == cur.GetName()).Cost;//Синхронизация цены с StockExchange
                output += cur.GetCur();
                output += "\n";
            }
            if(output == "")
                output = "Пусто";
            return output;
        }
        public StockExchange GetStockExchange()
        {
            return st;
        }

        //player interact
        public void ShowMenu()
        {
            Menu.ShowMenu(new Menu[]
            {
                new Menu("Начать игру", delegate(PlayerInterface sender){ sender.ShowGame(); }),
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
                            sender.st.ShowQuotations();
                }),
                new Menu("Купить/Продать", delegate(PlayerInterface sender){
                            sender.st.AddRandomOrders();
                            sender.st.ShowOrders(startCur[0], startCur[5]);
                            sender.st.RemoveRandomOrders(this);
                }),
                new Menu("Личный кабинет", delegate(PlayerInterface sender){ Console.WriteLine(GetCurOwned()); }),
                new Menu("Главное меню", delegate(PlayerInterface sender){ sender.MenuExit = true; })
            }, this);
        }
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
}
