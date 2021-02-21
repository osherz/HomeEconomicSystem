using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    enum States
    {
        MainState,
        Draft,
        GraphTypeChoosing,
        GraphSubjectChoosing,
        GraphSubSubjectChoosing,
        GraphMeasureChoosing,
        GraphRangeChoosing,
        SavingNewGraph,
        NewGraphSaved,
        SavingChanges,
        AddingToFavorites,
        AddedToFavorites,
        ClearingAll,
        Cleared,
        FavoritesEditing,
        AssociationRules,
        Favorites,
        ChangesSaved
    }

    enum Triggers
    {
        DraftSelected,
        FavoriteSelected,
        CreateGraph,
        AddToFavorites,
        Next,
        Back,
        ClearAll,
        AllCleared,
        SaveChanges,
        Edit,
        Cancel,
        AssociationRulesSelected,
        Finish
    }
    internal class DataAnalysisStateMachine : BaseStateMachine<States, Triggers>
    {
        public DataAnalysisStateMachine(IReadOnlyDictionary<States, Action> stateActionDict) : base(States.Favorites, stateActionDict)
        {
            Configure(States.MainState)
                .OnEntry(GetStateAction(States.MainState))
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
                .OnEntry(GetStateAction(States.Draft))
                .Permit(Triggers.CreateGraph, States.GraphTypeChoosing)
                .Permit(Triggers.AddToFavorites, States.AddingToFavorites);

            Configure(States.GraphTypeChoosing)
                .OnEntry(GetStateAction(States.GraphTypeChoosing))
                .Permit(Triggers.Next, States.GraphSubjectChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.GraphSubjectChoosing)
                .OnEntry(GetStateAction(States.GraphSubjectChoosing))
                .Permit(Triggers.Next, States.GraphSubSubjectChoosing)
                .Permit(Triggers.Back, States.GraphTypeChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.GraphSubSubjectChoosing)
                .OnEntry(GetStateAction(States.GraphSubSubjectChoosing))
                .Permit(Triggers.Next, States.GraphMeasureChoosing)
                .Permit(Triggers.Back, States.GraphSubjectChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.GraphRangeChoosing)
                .OnEntry(GetStateAction(States.GraphRangeChoosing))
                .Permit(Triggers.Finish, States.SavingNewGraph)
                .Permit(Triggers.Back, States.GraphSubjectChoosing)
                .Permit(Triggers.Cancel, States.Draft);

            Configure(States.SavingNewGraph)
                .OnEntry(GetStateAction(States.SavingNewGraph))
                .Permit(Triggers.Finish, States.NewGraphSaved);

            Configure(States.NewGraphSaved)
                .SubstateOf(States.Draft)
                .OnEntry(GetStateAction(States.NewGraphSaved));

            Configure(States.AddingToFavorites)
                .OnEntry(GetStateAction(States.AddingToFavorites))
                .Permit(Triggers.Finish, States.AddedToFavorites);

            Configure(States.AddedToFavorites)
                .SubstateOf(States.Draft)
                .OnEntry(GetStateAction(States.AddedToFavorites));
        }

        private void ConfigureFavorites()
        {
            Configure(States.Favorites)
                .SubstateOf(States.MainState)
                .OnEntry(GetStateAction(States.Favorites))
                .Permit(Triggers.Edit, States.FavoritesEditing);

            Configure(States.FavoritesEditing)
                .OnEntry(GetStateAction(States.FavoritesEditing))
                .Permit(Triggers.ClearAll, States.ClearingAll)
                .Permit(Triggers.Cancel, States.Favorites)
                .Permit(Triggers.SaveChanges, States.SavingChanges);

            Configure(States.ClearingAll)
                .OnEntry(GetStateAction(States.ClearingAll))
                .Permit(Triggers.AllCleared, States.Cleared);

            Configure(States.SavingChanges)
                .OnEntry(GetStateAction(States.SavingChanges))
                .Permit(Triggers.Finish, States.ChangesSaved);

            Configure(States.Cleared)
                .SubstateOf(States.Favorites)
                .OnEntry(GetStateAction(States.Cleared));

            Configure(States.ChangesSaved)
                .SubstateOf(States.Favorites)
                .OnEntry(GetStateAction(States.ChangesSaved));
        }

        private void ConfigureAssociationRules()
        {
            Configure(States.AssociationRules)
                .SubstateOf(States.MainState);
        }
    }
}
