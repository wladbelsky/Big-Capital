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
            PlayerInterface.Instance.ReadName();
            PlayerInterface.Instance.AddCurOwned(new CurOwned(StockExchange.Instance.GetQuotations()[0], 100));
            PlayerInterface.Instance.ShowMenu();
            //PlayerInterface.Save("capital.sav");
            System.Diagnostics.Debug.WriteLine("Завершено!");  //debug console
        }
    }
}
