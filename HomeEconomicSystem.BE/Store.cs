using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Transaction> Transactions { get; private set; }
    }
}
