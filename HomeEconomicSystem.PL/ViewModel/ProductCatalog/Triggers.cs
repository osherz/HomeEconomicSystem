using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.ProductCatalog
{
    internal enum Triggers
    {
        Edit,
        EndEdit,
        Search, 
        SearchFailed, 
        SearchSucceeded,
        Delete, 
        Create, 
        Select,
        DeSelect,
        CatalogSelected,
        SaveChanges
    }
}
