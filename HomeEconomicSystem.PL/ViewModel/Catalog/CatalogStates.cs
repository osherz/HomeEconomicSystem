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

        ProductCatalog, 
        SearchingProduct,
        SearchProductComplete,
        EditingProduct,
        ProductDeleting,
        ProductDeleted,
        SavingProduct,
        SavingProductComplete,

        CategoryCatalog,
        SearchingCategory,
        SearchCategoryComplete,
        EditingCategory,
        CategoryDeleting,
        CategoryDeleted,
        SavingCategory,
        SavingCategoryComplete
    }
}
