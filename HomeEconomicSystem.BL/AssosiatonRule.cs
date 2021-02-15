using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL
{
    internal class AssosiatonRule : IAssosiatonRule
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Product> GoesWith { get; set; }
        public float Probability { get; set; }
    }
}
