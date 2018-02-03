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
            PlayerInterface playerInterface = new PlayerInterface(PlayerInterface.ReadName(), new CurOwned(PlayerInterface.startCur[0], 100));
            //playerInterface.AddCurOwned(new CurOwned(playerInterface.GetStockExchange().GetQuotations()[0], 100));
            playerInterface.ShowMenu();
            //PlayerInterface.Save("capital.sav", playerInterface);
            System.Diagnostics.Debug.WriteLine("Завершено!");  //debug console
        }
    }
}
