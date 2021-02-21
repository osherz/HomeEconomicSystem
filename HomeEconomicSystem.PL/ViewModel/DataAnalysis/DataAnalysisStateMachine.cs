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
            Configure(States.MainState)
                .OnEntry(GetStateEntryAction(States.MainState))
                .OnExit(GetStateExitAction(States.MainState))
                .Permit(Triggers.DraftSelected, States.Draft)
                .Permit(Triggers.FavoriteSelected, States.Favorites)
                .Permit(Triggers.AssociationRulesSelected, States.AssociationRules);

            ConfigureDraft();
            ConfigureFavorites();
            ConfigureAssociationRules();
        }

        private void ConfigureDraft()
        {
            Configure(States.Draft)
                .SubstateOf(States.MainState)
                .OnEntry(GetStateEntryAction(States.Draft))
                .OnExit(GetStateExitAction(States.Draft))
                .Permit(Triggers.CreateGraph, States.GraphTypeChoosing)
                .Permit(Triggers.AddToFavorites, States.AddingToFavorites);

            Configure(States.GraphTypeChoosing)
                .OnEntry(GetStateEntryAction(States.GraphTypeChoosing))
                .OnExit(GetStateExitAction(States.GraphTypeChoosing))
                .Permit(Triggers.Next, States.GraphSubjectChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.GraphSubjectChoosing)
                .OnEntry(GetStateEntryAction(States.GraphSubjectChoosing))
                .OnExit(GetStateExitAction(States.GraphSubjectChoosing))
                .Permit(Triggers.Next, States.GraphSubSubjectChoosing)
                .Permit(Triggers.Back, States.GraphTypeChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.GraphSubSubjectChoosing)
                .OnEntry(GetStateEntryAction(States.GraphSubSubjectChoosing))
                .OnExit(GetStateExitAction(States.GraphSubSubjectChoosing))
                .Permit(Triggers.Next, States.GraphMeasureChoosing)
                .Permit(Triggers.Back, States.GraphSubjectChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.GraphMeasureChoosing)
                .OnEntry(GetStateEntryAction(States.GraphMeasureChoosing))
                .OnExit(GetStateExitAction(States.GraphMeasureChoosing))
                .Permit(Triggers.Next, States.GraphRangeChoosing)
                .Permit(Triggers.Back, States.GraphSubSubjectChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.GraphRangeChoosing)
                .OnEntry(GetStateEntryAction(States.GraphRangeChoosing))
                .OnExit(GetStateExitAction(States.GraphRangeChoosing))
                .Permit(Triggers.Finish, States.SavingNewGraph)
                .Permit(Triggers.Back, States.GraphMeasureChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.SavingNewGraph)
                .OnEntry(GetStateEntryAction(States.SavingNewGraph))
                .OnExit(GetStateExitAction(States.SavingNewGraph))
                .Permit(Triggers.Finish, States.NewGraphSaved);

            Configure(States.NewGraphSaved)
                .SubstateOf(States.Draft)
                .OnEntry(GetStateEntryAction(States.NewGraphSaved))
                .OnExit(GetStateExitAction(States.NewGraphSaved));

            Configure(States.AddingToFavorites)
                .OnEntry(GetStateEntryAction(States.AddingToFavorites))
                .Permit(Triggers.Finish, States.AddedToFavorites)
                .OnExit(GetStateExitAction(States.AddedToFavorites));

            Configure(States.AddedToFavorites)
                .SubstateOf(States.Draft)
                .OnEntry(GetStateEntryAction(States.AddedToFavorites))
                .OnExit(GetStateExitAction(States.AddedToFavorites));
        }

        private void ConfigureFavorites()
        {
            Configure(States.Favorites)
                .SubstateOf(States.MainState)
                .OnEntry(GetStateEntryAction(States.Favorites))
                .OnExit(GetStateExitAction(States.Favorites))
                .Permit(Triggers.Edit, States.FavoritesEditing);

            Configure(States.FavoritesEditing)
                .OnEntry(GetStateEntryAction(States.FavoritesEditing))
                .OnExit(GetStateExitAction(States.FavoritesEditing))
                .Permit(Triggers.ClearAll, States.ClearingAll)
                .Permit(Triggers.Cancel, States.Favorites)
                .Permit(Triggers.SaveChanges, States.SavingChanges);

            Configure(States.ClearingAll)
                .OnEntry(GetStateEntryAction(States.ClearingAll))
                .OnExit(GetStateExitAction(States.ClearingAll))
                .Permit(Triggers.AllCleared, States.Cleared);

            Configure(States.SavingChanges)
                .OnEntry(GetStateEntryAction(States.SavingChanges))
                .OnExit(GetStateExitAction(States.SavingChanges))
                .Permit(Triggers.Finish, States.ChangesSaved);

            Configure(States.Cleared)
                .SubstateOf(States.Favorites)
                .OnEntry(GetStateEntryAction(States.Cleared))
                .OnExit(GetStateExitAction(States.Cleared));

            Configure(States.ChangesSaved)
                .SubstateOf(States.Favorites)
                .OnEntry(GetStateEntryAction(States.ChangesSaved))
                .OnExit(GetStateExitAction(States.ChangesSaved));
        }

        private void ConfigureAssociationRules()
        {
            Configure(States.AssociationRules)
                .SubstateOf(States.MainState)
                .OnEntry(GetStateEntryAction(States.AssociationRules))
                .OnExit(GetStateExitAction(States.AssociationRules));
        }
    }
}
