using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Big_Capital.Capital_Logic;

namespace Big_Capital
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerInterface playerInterface = new PlayerInterface();
            playerInterface.addCurOwned(new CurOwned("name", 10, 100));
            playerInterface.ShowMenu();
            Console.WriteLine("Завершено!");
            Console.ReadKey();
        }
    }
}
