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
        DraftView _draftView { get; set; }

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
                {
                    States.Draft, ()=>
                    {
                        if(_draftView is null)
                        {
                            _draftView = new DraftView() {DataContext = draftVM};
                        }
                        Content = _draftView;
                    }
                },
                { States.Favorites, () => Content = new FavoritesView() },
                { States.AssociationRules ,() => Content = new AssociationRulesView() },
            }.Concat(draftVM.GetStatesEntryAction()).ToDictionary(item => item.Key, item => item.Value);

            var statesExitAction = draftVM.GetStatesExitAction();

            StateMachine = new(statesEntryAction, statesExitAction);

            draftVM.SetStateMachine(StateMachine);
        }
    }
}
