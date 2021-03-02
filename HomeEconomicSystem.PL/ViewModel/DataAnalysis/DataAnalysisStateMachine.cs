using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class DataAnalysisStateMachine : BaseStateMachine<States, Triggers>
    {
        public DataAnalysisStateMachine(IReadOnlyDictionary<States, Action> stateActionDict, IReadOnlyDictionary<States, Action> stateExitActionDict = null) : base(States.Favorites, stateActionDict, stateExitActionDict)
        {
            BasicConfigure(States.MainState)
                //.Permit(Triggers.DraftSelected)
                .Permit(Triggers.FavoriteSelected, States.Favorites)
                .Permit(Triggers.AssociationRulesSelected, States.AssociationRules);

            ConfigureDraft();
            ConfigureFavorites();
            ConfigureAssociationRules();
        }

        private void ConfigureDraft()
        {
            BasicConfigure(States.Draft, States.MainState)
                .Permit(Triggers.CreateGraph, States.GraphCreatingForDraft)
                .Permit(Triggers.AddToFavorites, States.AddingToFavorites);

            BasicConfigure(States.GraphCreatingForDraft)
                .Permit(Triggers.Cancel, States.DraftCreatingCanceled)
               .Permit(Triggers.Finish, States.SavingNewGraphDraft);

            BasicConfigure(States.DraftCreatingCanceled, States.Draft);

            BasicConfigure(States.SavingNewGraphDraft)
                .Permit(Triggers.Finish, States.NewGraphDraftSaved);

            BasicConfigure(States.NewGraphDraftSaved, States.Draft);

            BasicConfigure(States.AddingToFavorites)
                .Permit(Triggers.Finish, States.AddedToFavorites);

            BasicConfigure(States.AddedToFavorites, States.Draft);
        }

        private void ConfigureFavorites()
        {
            BasicConfigure(States.Favorites, States.MainState)
                //.Permit(Triggers.Edit, States.FavoritesEditing)
                .Permit(Triggers.Delete, States.DeleteFavorite)
                .Permit(Triggers.CreateGraph, States.GraphCreatingForFavorite);

            BasicConfigure(States.DeleteFavorite)
                .Permit(Triggers.Finish, States.FavoriteDeleted);

            BasicConfigure(States.FavoriteDeleted, States.Favorites);

            BasicConfigure(States.GraphCreatingForFavorite)
                .Permit(Triggers.Cancel, States.FavoriteCreatingCanceled)
               .Permit(Triggers.Finish, States.SavingNewGraphFavorite);

            BasicConfigure(States.FavoriteCreatingCanceled, States.Favorites);

            BasicConfigure(States.SavingNewGraphFavorite)
                .Permit(Triggers.Finish, States.NewGraphFavoriteSaved);

            BasicConfigure(States.NewGraphFavoriteSaved, States.Favorites);

            BasicConfigure(States.FavoritesEditing)
                .Permit(Triggers.ClearAll, States.ClearingAll)
                .Permit(Triggers.Cancel, States.Favorites)
                .Permit(Triggers.SaveChanges, States.SavingChanges);

            BasicConfigure(States.ClearingAll)
                .Permit(Triggers.AllCleared, States.Cleared);

            BasicConfigure(States.SavingChanges)
                .Permit(Triggers.Finish, States.ChangesSaved);

            BasicConfigure(States.Cleared, States.Favorites);

            BasicConfigure(States.ChangesSaved, States.Favorites);
        }

        private void ConfigureAssociationRules()
        {
            BasicConfigure(States.AssociationRules, States.MainState)
                .Permit(Triggers.Load, States.AssociationRulesGraph)
                .PermitReentry(Triggers.AssociationRulesSelected);

            BasicConfigure(States.AssociationRulesGraph, States.AssociationRules)
                .Permit(Triggers.Switch, States.AssociationRulesTable);

            BasicConfigure(States.AssociationRulesTable, States.AssociationRules)
                .Permit(Triggers.Switch, States.AssociationRulesGraph);
        }
    }
}
