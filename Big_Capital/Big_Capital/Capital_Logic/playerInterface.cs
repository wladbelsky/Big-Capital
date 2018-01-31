﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class PlayerInterface
    {
        String name;
        CurOwned[] wallet;
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
        stock_exchange st = new stock_exchange(startCur);
        public PlayerInterface()
        {
            ReadName();
            wallet = new CurOwned[0];
        }
        public PlayerInterface(String name, CurOwned own)
        {
            wallet = new CurOwned[1];
            wallet[0] = own;
            this.name = name;
        }
        public PlayerInterface(String name, CurOwned[] own)
        {
            this.name = name;
            wallet = own;
        }
        public void ReadName()
        {
            Console.Write("Введите имя игрока: ");
            name = Console.ReadLine();

        }

        //wallet interact
        public void ShowWallet()
        {
            for(int i = 0; i < wallet.Length; i++)
            {
                Console.WriteLine(GetCurOwned());
            }
        }
        public void addCurOwned(CurOwned c)
        {
            Array.Resize(ref wallet, wallet.Length + 1);
            wallet[wallet.Length - 1] = c;
        }
        public String GetCurOwned()
        {
            String output = "";
            for(int i = 0; i < wallet.Length; i++)
            {
                output += wallet[i].GetCur();
                output += "\n";
            }
            if(output == "")
                output = "Пусто";
            return output;
        }

        //player interact
        public void ShowMenu()
        {
            Boolean cont = true;
            while(cont)
            {
                Console.WriteLine("1)Начать игру\n2)Настройки\n3)Выход");

                switch(Console.ReadKey().KeyChar.ToString())
                {
                    case "1":
                        {
                            //to do!
                            ShowGame();
                            break;
                        }
                    case "2":
                        {
                            //to do!
                            Console.WriteLine("Settings?");
                            break;
                        }
                    case "3":
                        {
                            //Environment.Exit(0);
                            cont = false;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                Console.WriteLine();
            }
        }

        private void ShowGame()
        {
            Boolean cont = true;
            while (cont)
            {
                //Console.WriteLine("\nИмя игрока: " + name + "\nВ процессе... Любая кнопка чтобы вернутся в меню.");
                Console.WriteLine("\n1)Показать котировки\n2)Купить/Продать\n3)Личный кабинет");
                switch (Console.ReadKey().KeyChar.ToString())
                {
                    case "1":
                        {
                            st.RandomOrders();
                            st.ShowOrders(startCur[0],);
                        }
                    case "3":
                        {
                            Console.WriteLine(GetCurOwned());
                            break;
                        }
                    default:
                        {
                            cont = false;
                            break;
                        }
                }
            }
        }
    }

    class CurOwned : Currency   //Валюта пользователя
    {
        Double count;

        public CurOwned(Currency cur, Double own)
        {
            name = cur.GetName();
            Cost = cur.Cost;
        }
        public CurOwned(String n, Double own , Double cost = 0) : base(n, cost)
        {
            count = own;
        }
        public override string GetCur()
        {
            return base.GetCur() + "\t" + Owned;
        }
        public Double Owned
        {
            get { return count; }
            set
            {
                if (value > 0)
                    count = value;
                else
                    count = 0;
            }
        }
    }
}
