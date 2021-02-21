using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    enum States
    {
        Draft,
        GraphTypeChoosing,
        GraphSubjectChoosing,
        GraphSubSubjectChoosing,
        GraphMeesureChoosing,
        GraphRangeChoosing,
        SavingNewGrapth,
        SavingChanges,
        DeletingAll,
        FavoritesEditing,
        AssociationRules,
        Favorites
    }

    enum Triggers
    {
        DraftSelected,
        FavoriteSelected,
        GraphTypeChoosed,
        GraphSubSubjectChoosed,
        GraphMeesureChoosed,
        GraphRangeChoosed,
        NewGraphSaved,
        ChangesSaved,
        DeleteAll,
        AllDeleted,
        SaveChanges,
        Edit,
        AssociationRulesSelected
    }
    internal class DataAnalysisStateMachine : Stateless.StateMachine<States, Triggers>
    {
        IReadOnlyDictionary<States, Action> _stateActionDict;

        public DataAnalysisStateMachine() : base(States.Favorites)
        {
            
        }
    }
}
