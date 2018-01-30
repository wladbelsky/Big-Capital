using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class stock_exchange
    {
        order[] order_sell;
        order[] order_buy;
        currency[] quotations;
        public stock_exchange(currency[] c)
        {
            this.quotations = c;
        }
        public void Show()
        {
            Console.Write("\n\tВалютные пары:\n\tНаименование:\t\t\tЦена:");
            for (int i = 0; i < quotations.Length; i++)
                Console.Write(i + " " + quotations[i].GetName() + "\t\t\t" + quotations[i].Cost);
        }
        public void Trade()
        {
            Int32 cur1;
            Int32 cur2;
            Show();
            Console.Write("\tДля начала торгов введите номер 1 валюты и номер 2 валюты через пробел");
            string[] tokens = Console.ReadLine().Split();
            Int32 one = Convert.ToInt32(tokens[0]);
            Int32 two = Convert.ToInt32(tokens[1]);
            Console.Writeline();
        }
    }
    class order
    {
        Double count;
        Currency cur1;
        Currency cur2;
        public order(Double count, Currency cur1, Currency cur2)
        {
            this.cur1 = cur1;
            this.count = count;
            this.cur2 = cur2;
        }
    }
}
