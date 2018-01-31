using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class Currency
    {
        protected String name;
        Double cost;
        public Currency()
        {
            name = "";
            cost = 0;
        }
        public Currency(String n, Double cost = 0)
        {
            name = n;
            this.cost = cost;
        }

        public Double Cost
        {
            get { return cost; }
            set
            {
                if (value > 0)
                    cost = value;
                else
                    cost = 0;
            }
        }

        public string GetCur()
        {
           return GetName() + "\t\t\t" + Cost;
        }

        public String GetName()
        {
            return name;
        }

    }
}
