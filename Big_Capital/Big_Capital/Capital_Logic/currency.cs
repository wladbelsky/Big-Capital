using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class Currency
    {
        protected string name;
        Double cost;
        public Currency()
        {
            name = "";
            cost = 0;
        }
        public Currency(string n, Double cost = 0)
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

        public string GetName()
        {
            return name;
        }

    }
}
