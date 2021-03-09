using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.Catalog
{
    public enum CatalogTriggers
    {
        Edit,
        Finish,
        Search, 
        SearchFailed, 
        SearchSucceeded,
        Delete, 
        Create,
        Cancel,
        ProductCatalogSelected,
        CategoryCatalogSelected
    }
}
