using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.Catalog
{
    public class CatalogStateMachine : BaseStateMachine<CatalogStates, CatalogTriggers>
    {
        public CatalogStateMachine(IReadOnlyDictionary<CatalogStates, Action> stateActionDict) : base(CatalogStates.ProductCatalog, stateActionDict)
        {
            BasicConfigure(CatalogStates.MainState)
                .Permit(CatalogTriggers.ProductCatalogSelected, CatalogStates.ProductCatalog)
                .Permit(CatalogTriggers.CategoryCatalogSelected, CatalogStates.CategoryCatalog);

            ConfigureProductCatalog();
            ConfigureCategoryCatalog();

        }

        private void ConfigureProductCatalog()
        {
            BasicConfigure(CatalogStates.ProductCatalog,CatalogStates.MainState)
                .Permit(CatalogTriggers.Search, CatalogStates.SearchingProduct)
                .Permit(CatalogTriggers.Select, CatalogStates.ProductSelected);

            BasicConfigure(CatalogStates.SearchingProduct, CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.SearchProductComplete,CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.ProductSelected,CatalogStates.ProductCatalog)
                 .Permit(CatalogTriggers.DeSelect, CatalogStates.ProductNoSelected)
                 .Permit(CatalogTriggers.Edit, CatalogStates.EditingProduct);

            BasicConfigure(CatalogStates.ProductNoSelected,CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.EditingProduct,CatalogStates.ProductCatalog)
                 .Permit(CatalogTriggers.EndEdit, CatalogStates.ProductSelected)
                 .Permit(CatalogTriggers.SaveChanges, CatalogStates.SavingChangesCatalog);

        }
        private void ConfigureCategoryCatalog()
        {
            BasicConfigure(CatalogStates.CategoryCatalog,CatalogStates.MainState)
                .Permit(CatalogTriggers.Search, CatalogStates.SearchingCategory)
                .Permit(CatalogTriggers.Select, CatalogStates.CategorySelected)
                .Permit(CatalogTriggers.Create, CatalogStates.CategoryCreated);

            BasicConfigure(CatalogStates.SearchingCategory,CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.SearchCategoryComplete,CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.CategorySelected,CatalogStates.CategoryCatalog)
                 .Permit(CatalogTriggers.DeSelect, CatalogStates.CategoryNoSelected)
                 .Permit(CatalogTriggers.Edit, CatalogStates.EditingCategory);

            BasicConfigure(CatalogStates.CategoryNoSelected,CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.EditingCategory,CatalogStates.CategoryCatalog)
                 .Permit(CatalogTriggers.EndEdit, CatalogStates.CategorySelected)
                 .Permit(CatalogTriggers.Delete, CatalogStates.CategoryDeleted)
                 .Permit(CatalogTriggers.SaveChanges, CatalogStates.SavingChangesCatalog);

            BasicConfigure(CatalogStates.CategoryCreated,CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.CategoryDeleted,CatalogStates.CategoryCatalog);

        }
    }
}
