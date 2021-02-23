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
        DraftView _draftView { get; set; }
        FavoritesView _favoritesView { get; set; }

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
            FavoritesVM favoritesVM = new FavoritesVM();

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
                { 
                    States.Favorites, () =>
                    {
                        if(_favoritesView is null)
                        {
                            _favoritesView = new FavoritesView() {DataContext = favoritesVM};
                        }
                        Content = _favoritesView;
                    }
                },
                { States.AssociationRules ,() => Content = new AssociationRulesView() },
            }.Concat(draftVM.GetStatesEntryAction()).ToDictionary(item => item.Key, item => item.Value);

            var statesExitAction = draftVM.GetStatesExitAction();

            StateMachine = new(statesEntryAction, statesExitAction);

            draftVM.SetStateMachine(StateMachine);
            StateMachine.Fire(Triggers.FavoriteSelected);
        }
    }
}
