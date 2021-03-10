using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.Catalog
{
    public class CatalogStateMachine : BaseStateMachine<CatalogStates, CatalogTriggers>
    {
        public CatalogStateMachine(IReadOnlyDictionary<CatalogStates, Action> stateEntryActionDict, IReadOnlyDictionary<CatalogStates, Action> stateExitActionDict) : base(CatalogStates.MainState, stateEntryActionDict, stateExitActionDict)
        {
            BasicConfigure(CatalogStates.MainState)
                .Permit(CatalogTriggers.ProductCatalogSelected, CatalogStates.ProductCatalog)
                .Permit(CatalogTriggers.CategoryCatalogSelected, CatalogStates.CategoryCatalog);

            ConfigureProductCatalog();
            ConfigureCategoryCatalog();

        }

        private void ConfigureProductCatalog()
        {
            BasicConfigure(CatalogStates.ProductCatalog)
                .Permit(CatalogTriggers.Search, CatalogStates.SearchingProduct)
                .Permit(CatalogTriggers.Edit, CatalogStates.EditingProduct)
                .Permit(CatalogTriggers.Delete, CatalogStates.ProductDeleting)
                .Permit(CatalogTriggers.Create, CatalogStates.ProductCreating)
                .Permit(CatalogTriggers.CategoryCatalogSelected, CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.SearchingProduct, CatalogStates.ProductCatalog)
                .Permit(CatalogTriggers.SearchSucceeded, CatalogStates.SearchProductComplete);

            BasicConfigure(CatalogStates.SearchProductComplete,CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.EditingProduct)
                 .Permit(CatalogTriggers.Finish, CatalogStates.SavingProduct)
                 .Permit(CatalogTriggers.Cancel, CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.SavingProduct)
                .Permit(CatalogTriggers.Finish, CatalogStates.SavingProductComplete);

            BasicConfigure(CatalogStates.SavingProductComplete, CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.ProductDeleting)
                .Permit(CatalogTriggers.Finish, CatalogStates.ProductDeleted);

            BasicConfigure(CatalogStates.ProductDeleted, CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.ProductCreating)
                .Permit(CatalogTriggers.Finish, CatalogStates.SavingProduct)
                .Permit(CatalogTriggers.Cancel, CatalogStates.ProductCatalog);
        }
        private void ConfigureCategoryCatalog()
        {
            BasicConfigure(CatalogStates.CategoryCatalog)
                .Permit(CatalogTriggers.Search, CatalogStates.SearchingCategory)
                .Permit(CatalogTriggers.Edit, CatalogStates.EditingCategory)
                .Permit(CatalogTriggers.Delete, CatalogStates.CategoryDeleting)
                .Permit(CatalogTriggers.Create, CatalogStates.CategoryCreating)
                .Permit(CatalogTriggers.ProductCatalogSelected, CatalogStates.ProductCatalog);

            BasicConfigure(CatalogStates.SearchingCategory, CatalogStates.CategoryCatalog)
                .Permit(CatalogTriggers.SearchSucceeded, CatalogStates.SearchCategoryComplete);

            BasicConfigure(CatalogStates.SearchCategoryComplete, CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.EditingCategory)
                 .Permit(CatalogTriggers.Finish, CatalogStates.SavingCategory)
                 .Permit(CatalogTriggers.Cancel, CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.SavingCategory)
                .Permit(CatalogTriggers.Finish, CatalogStates.SavingCategoryComplete);

            BasicConfigure(CatalogStates.SavingCategoryComplete, CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.CategoryDeleting)
                .Permit(CatalogTriggers.Finish, CatalogStates.CategoryDeleted);

            BasicConfigure(CatalogStates.CategoryDeleted, CatalogStates.CategoryCatalog);

            BasicConfigure(CatalogStates.CategoryCreating)
                .Permit(CatalogTriggers.Finish, CatalogStates.SavingCategory)
                .Permit(CatalogTriggers.Cancel, CatalogStates.CategoryCatalog);

        }
    }
}
