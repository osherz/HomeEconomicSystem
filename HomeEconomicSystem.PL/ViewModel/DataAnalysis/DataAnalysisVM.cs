using HomeEconomicSystem.PL.View;
using HomeEconomicSystem.PL.ViewModel.PageDisplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class DataAnalysisVM : VMBasic<States, Triggers>
    {
        public DataAnalysisVM(IPageDisplay parentPageDisplay) : base(parentPageDisplay)
        {
            StateMachine.Fire(Triggers.FavoriteSelected);
        }

        protected override BaseStateMachine<States, Triggers> CreateStateMachine(Dictionary<States, Action> statesEntryAction, Dictionary<States, Action> statesExitAction)
        {
            return new DataAnalysisStateMachine(statesEntryAction, statesExitAction);
        }

        protected override Dictionary<States, Func<UserControl>> CreateViewsCreation()
        {
            return new Dictionary<States, Func<UserControl>>
            {
                 {States.Favorites, () =>  new FavoritesView() },
                 {States.Draft, () => new DraftView() },
                {States.AssociationRules, () => new AssociationRulesView() },
            };
        }


        protected override Dictionary<States, IInnerVM<States, Triggers>> CreateInnerVM()
        {
            return new Dictionary<States, IInnerVM<States, Triggers>>
            {
                {States.Favorites, new FavoritesVM(ParentPageDisplay) },
                 {States.Draft, new DraftVM(ParentPageDisplay) },
                {States.AssociationRules, new AssociationRulesVM(ParentPageDisplay) }
            };
        }
    }
}
