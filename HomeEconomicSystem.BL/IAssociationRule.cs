using HomeEconomicSystem.BE;
using System.Collections.Generic;

namespace HomeEconomicSystem.BL
{
    public interface IAssociationRule
    {
        IEnumerable<Product> GoesWith { get; set; }
        float Probability { get; set; }
        IEnumerable<Product> Product { get; set; }
    }
}