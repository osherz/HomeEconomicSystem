using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.ProductCatalog
{
    internal class ProductCatalogStateMachine : BaseStateMachine<States, Triggers>
    {
        public ProductCatalogStateMachine(IReadOnlyDictionary<States, Action> stateActionDict) : base(States.ProductCatalog, stateActionDict)
        {
            Configure(States.MainState)
                .OnEntry(GetStateEntryAction(States.MainState))
                .Permit(Triggers.ProductCatalogSelected, States.CategoryCatalog);

            ConfigureProductCatalog();
            ConfigureCategoryCatalog();

        }

        private void ConfigureProductCatalog()
        {
            Configure(States.ProductCatalog)
                .SubstateOf(States.MainState)
                .OnEntry(GetStateEntryAction(States.ProductCatalog))
                .Permit(Triggers.Search, States.SearchingProduct)
                .Permit(Triggers.Select, States.ProductSelected);

            Configure(States.SearchingProduct)
                 .SubstateOf(States.ProductCatalog)
                 .OnEntry(GetStateEntryAction(States.SearchingProduct));

            Configure(States.SearchProductComplete)
                 .SubstateOf(States.ProductCatalog)
                 .OnEntry(GetStateEntryAction(States.SearchComplete));

            Configure(States.ProductSelected)
                 .SubstateOf(States.ProductCatalog)
                 .OnEntry(GetStateEntryAction(States.ProductSelected))
                 .Permit(Triggers.DeSelect, States.ProductNoSelected)
                 .Permit(Triggers.Edit, States.EditingProduct);

            Configure(States.ProductNoSelected)
                 .SubstateOf(States.ProductCatalog)
                 .OnEntry(GetStateEntryAction(States.ProductNoSelected));

            Configure(States.EditingProduct)
                 .SubstateOf(States.ProductCatalog)
                 .OnEntry(GetStateEntryAction(States.ProductNoSelected))
                 .Permit(Triggers.EndEdit, States.ProductSelected)
                 .Permit(Triggers.SaveChanges, States.SavingChangesCatalog);

        }
        private void ConfigureCategoryCatalog()
        {
            Configure(States.CategoryCatalog)
                .SubstateOf(States.MainState)
                .OnEntry(GetStateEntryAction(States.CategoryCatalog))
                .Permit(Triggers.Search, States.SearchingCategory)
                .Permit(Triggers.Select, States.CategorySelected)
                .Permit(Triggers.Create, States.CategoryCreated);

            Configure(States.SearchingCategory)
                 .SubstateOf(States.CategoryCatalog)
                 .OnEntry(GetStateEntryAction(States.SearchingCategory));

            Configure(States.SearchCategoryComplete)
                 .SubstateOf(States.CategoryCatalog)
                 .OnEntry(GetStateEntryAction(States.SearchComplete));

            Configure(States.CategorySelected)
                 .SubstateOf(States.CategoryCatalog)
                 .OnEntry(GetStateEntryAction(States.CategorySelected))
                 .Permit(Triggers.DeSelect, States.CategoryNoSelected)
                 .Permit(Triggers.Edit, States.EditingCategory);

            Configure(States.CategoryNoSelected)
                 .SubstateOf(States.CategoryCatalog)
                 .OnEntry(GetStateEntryAction(States.CategoryNoSelected));

            Configure(States.EditingCategory)
                 .SubstateOf(States.CategoryCatalog)
                 .OnEntry(GetStateEntryAction(States.CategoryNoSelected))
                 .Permit(Triggers.EndEdit, States.CategorySelected)
                 .Permit(Triggers.Delete, States.CategoryDeleted)
                 .Permit(Triggers.SaveChanges, States.SavingChangesCatalog);

            Configure(States.CategoryCreated)
                 .SubstateOf(States.CategoryCatalog)
                 .OnEntry(GetStateEntryAction(States.CategoryCreated));

            Configure(States.CategoryDeleted)
                 .SubstateOf(States.CategoryCatalog)
                 .OnEntry(GetStateEntryAction(States.CategoryDeleted));

        }
    }
}
