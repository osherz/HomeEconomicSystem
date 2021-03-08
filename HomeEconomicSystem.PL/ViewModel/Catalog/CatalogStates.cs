using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.Catalog
{
    public enum CatalogStates
    {
        MainState,
        SavingChangesCatalog,

        ProductCatalog, 
        SearchingProduct,
        ProductSelected,
        SearchProductComplete,
        ProductNoSelected,
        EditingProduct, 

        CategoryCatalog,
        SearchingCategory,
        CategorySelected,
        SearchCategoryComplete,
        CategoryNoSelected,
        EditingCategory,
        CategoryCreated,
        CategoryDeleted,
        SearchComplete
    }
}
