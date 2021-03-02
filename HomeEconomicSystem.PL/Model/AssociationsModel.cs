using HomeEconomicSystem.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.Model
{
    public class AssociationsModel
    {
        IBL _bl;
        IAssosiationProductsAnalysis _assosiationAnalysis;

        public IEnumerable<IAssociationRule> AssosiatonRules => _assosiationAnalysis.GetAssosiatonRules();

        public AssociationsModel()
        {
            _bl = new BL.BL();
            _assosiationAnalysis = _bl.AssosiationProductsAnalysis;
        }
    }
}
