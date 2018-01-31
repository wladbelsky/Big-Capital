using System;
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

        public PlayerInterface() => ReadName();
        public PlayerInterface(String name) => this.name = name;
        public PlayerInterface(String name, CurOwned[] own)
        {
            this.name = name;
            wallet = own;
        }
        public PlayerInterface(String name, Currency[] cur)
        {
            this.name = name;
            wallet = new CurOwned[cur.Length];
            for(int i = 0; i < cur.Length; i++)
            {
                wallet[i] = new CurOwned(cur[i], 0);
            }
        }
        public void ReadName()
        {
            Console.Write("Введите имя игрока: ");
            name = Console.ReadLine();

        }
        public void ShowWallet()
        {
            for(int i = 0; i < wallet.Length; i++)
            {
                Console.WriteLine("");//Вывод доступной валюты
                                      //Сделать универсальный метод
            }
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
                            Environment.Exit(0);
                            break;
                        }
                    default:
                        {
                            cont = false;
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
                Console.WriteLine("\nИмя игрока: " + name + "\nВ процессе... Любая кнопка чтобы вернутся в меню.");
                //1)Показать котировки\n2)Купить/Продать\n3)Мой счет
                switch (Console.ReadKey().KeyChar.ToString())
                {
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
