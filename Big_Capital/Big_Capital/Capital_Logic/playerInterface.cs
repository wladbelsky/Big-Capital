using System;
using System.Collections.Generic;
using System.Diagnostics; // Debug
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

        //wallet interact
        public void AddCurOwned(CurOwned c)
        {
            wallet.Add(c);
        }
        public String GetCurOwned()
        {
            String output = "";
            foreach(CurOwned cur in wallet)
            {
                output += cur.GetCur();
                output += "\n";
            }
            if(output == "")
                output = "Пусто";
            return output;
        }

        //player interact
        public void ShowMenu()
        {
            Menu.ShowMenu(new Menu[]
            {
                new Menu("Начать игру", delegate(PlayerInterface player){ player.ShowGame(); }),
                new Menu("Настройки", delegate(PlayerInterface player){ Console.WriteLine("Settings?"); }),
                new Menu("Выход", delegate(PlayerInterface player){  })
            }, this);
        }

        private void ShowGame()
        {
            Boolean cont = true;
            while (cont)
            {
                //Console.WriteLine("\nИмя игрока: " + name + "\nВ процессе... Любая кнопка чтобы вернутся в меню.");
                Console.WriteLine("\n1)Показать котировки\n2)Купить/Продать\n3)Личный кабинет\n0)Главное меню");
                switch (Console.ReadKey().KeyChar.ToString())
                {
                    case "1":
                        {
                            //st.ShowQuotations();
                            st.AddRandomOrders();
                            st.ShowOrders(startCur[0], startCur[5]);
                            st.RemoveRandomOrders();
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine(GetCurOwned());
                            break;
                        }
                    case "0":
                        {
                            cont = false;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
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
            while (true)
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
                    Debug.WriteLine(e.Message);
                    break;
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
