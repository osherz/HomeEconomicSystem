using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public enum States
    {
        MainState,
        SavingChangesCatalog

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
        CategoryDeleted
    }
}
