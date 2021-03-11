using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL
{
    internal class AssosiatonRule : IAssociationRule
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Product> GoesWith { get; set; }
        public double Probability { get; set; }

        public AssosiatonRule(IEnumerable<Product> product, IEnumerable<Product> goesWith, double probability)
        {
            Product = product;
            GoesWith = goesWith;
            Probability = probability;
        }
    }
}
