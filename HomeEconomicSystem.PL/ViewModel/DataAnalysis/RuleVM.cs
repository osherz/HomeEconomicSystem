using HomeEconomicSystem.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class RuleVM
    {
        public string Products { get; set; }
        public string GoesWith { get; set; }
        public string Probablity { get; set; }

        public RuleVM(IAssociationRule rule)
        {
            Products = string.Join(", ", rule.Product.Select(p=>p.Name));
            GoesWith = string.Join(", ", rule.GoesWith.Select(p => p.Name));
            Probablity = rule.Probability.ToString("P");
        }
    }
}
