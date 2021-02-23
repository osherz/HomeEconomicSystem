using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class DataAnalysisStateMachine : BaseStateMachine<States, Triggers>
    {
        private IEnumerable<States> _graphCreationOrder;

        public DataAnalysisStateMachine(IReadOnlyDictionary<States, Action> stateActionDict, IReadOnlyDictionary<States, Action> stateExitActionDict = null) : base(States.Favorites, stateActionDict, stateExitActionDict)
        {
            _graphCreationOrder = new[]
            {
                States.GraphSubjectChoosing,
                States.GraphTypeChoosing,
                States.GraphSubSubjectChoosing,
                States.GraphMeasureChoosing,
                States.GraphRangeChoosing
            };

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
                .Permit(Triggers.CreateGraph, _graphCreationOrder.First())
                .Permit(Triggers.AddToFavorites, States.AddingToFavorites);

            ConfigureGraphCreationStates();

            Configure(States.AddingToFavorites)
                .OnEntry(GetStateEntryAction(States.AddingToFavorites))
                .Permit(Triggers.Finish, States.AddedToFavorites)
                .OnExit(GetStateExitAction(States.AddedToFavorites));

            Configure(States.AddedToFavorites)
                .SubstateOf(States.Draft)
                .OnEntry(GetStateEntryAction(States.AddedToFavorites))
                .OnExit(GetStateExitAction(States.AddedToFavorites));
        }

        private void ConfigureGraphCreationStates()
        {
            var graphCreationOrder = _graphCreationOrder.ToArray();
            StateConfiguration config = null;
            for (int creationStateIndex = 0; creationStateIndex < _graphCreationOrder.Count(); creationStateIndex++)
            {
                States currentState = graphCreationOrder[creationStateIndex];
                // Config next of previous state.
                if (config != null)
                {   
                    config.Permit(Triggers.Next, currentState);
                }

                config = Configure(currentState)
                    .OnEntry(GetStateEntryAction(currentState))
                    .OnExit(GetStateExitAction(currentState))
                    .Permit(Triggers.Cancel, States.Draft);

                // Config back of current state.
                if (creationStateIndex > 0)
                {
                    States prev = graphCreationOrder[creationStateIndex - 1];
                    config.Permit(Triggers.Back, prev);
                }
            }
            config.Permit(Triggers.Finish, States.SavingNewGraph);

            Configure(States.SavingNewGraph)
                .OnEntry(GetStateEntryAction(States.SavingNewGraph))
                .OnExit(GetStateExitAction(States.SavingNewGraph))
                .Permit(Triggers.Finish, States.NewGraphSaved);

            Configure(States.NewGraphSaved)
                .SubstateOf(States.Draft)
                .OnEntry(GetStateEntryAction(States.NewGraphSaved))
                .OnExit(GetStateExitAction(States.NewGraphSaved));
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
