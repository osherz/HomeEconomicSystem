using HomeEconomicSystem.PL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class DataAnalysisVM : NotifyPropertyChanged
    {
        public DataAnalysisStateMachine StateMachine { get; }

        private UserControl _content;
        public UserControl Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public DataAnalysisVM()
        {
            DraftVM draftVM = new DraftVM();
            
            var statesEntryAction = new Dictionary<States, Action>{
                { States.Draft, ()=> Content = new DraftView(){ DataContext = draftVM } },
                { States.Favorites, () => Content = new FavoritesView() },
                { States.AssociationRules ,() => Content = new AssociationRulesView() },
            }.Concat(draftVM.GetStatesAction()).ToDictionary(item=>item.Key, item=>item.Value);
            
            var statesExitAction = draftVM.GetStatesExitAction();
            
            StateMachine = new(statesEntryAction, statesExitAction);

            draftVM.SetStateMachine(StateMachine);
        }
    }
}
