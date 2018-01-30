using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class PlayerInterface
    {
        string name;
        CurOwned[] wallet;

        public PlayerInterface(string name)
        {
            this.name = name;
        }
        public PlayerInterface(string name, CurOwned[] own)
        {
            this.name = name;
            wallet = own;
        }
        public PlayerInterface(string name, Currency[] cur)
        {
            this.name = name;
            wallet = new CurOwned[cur.Length];
            for(int i = 0; i < cur.Length; i++)
            {
                wallet[i] = new CurOwned(cur[i], 0);
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
        public CurOwned(string n, Double own , Double cost = 0) : base(n, cost)
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
