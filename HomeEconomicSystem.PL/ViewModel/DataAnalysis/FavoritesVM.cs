using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class FavoritesVM : NotifyPropertyChanged
    {
        public GraphCreationVM GraphCreationVM { get; }
        GraphsModel _graphsModel;
        DataAnalysisStateMachine _stateMachine;

        BasicGraph _graphToDelete;

        public IReadOnlyList<ViewModel.MenuItem> MenuItems { get; set; }

        private bool _creatingGraph;
        public bool CreatingGraph
        {
            get => _creatingGraph;
            set => SetProperty(ref _creatingGraph, value);
        }

        private ObservableCollection<BasicGraph> _graphsCollection;

        public ObservableCollection<BasicGraph> GraphsCollection
        {
            get { return _graphsCollection; }
            set { SetProperty(ref _graphsCollection, value); }
        }


        States _state;
        public States State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        #region Commands
        public ICommand CreateGraph { get; private set; }
        #endregion Commands

        public FavoritesVM()
        {
            GraphCreationVM = new GraphCreationVM();
            _graphsModel = new GraphsModel();
            LoadGraphs();
        }


        public void SetStateMachine(DataAnalysisStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.OnTransitionCompleted((e) => State = e.Destination);
            CreateGraph = stateMachine.CreateCommand(Triggers.CreateGraph);
            GraphCreationVM.Done += (sender, e) => stateMachine.Fire(Triggers.Finish);
            GraphCreationVM.Canceled += (sender, e) => stateMachine.Fire(Triggers.Cancel);

            MenuItems = new List<ViewModel.MenuItem>(new[]
            {
                //new ViewModel.MenuItem("ערוך", PackIconKind.Edit, _stateMachine.CreateCommand(Triggers.Edit)),
                new ViewModel.MenuItem("מחק", 
                                        PackIconKind.Delete, 
                                        _stateMachine.CreateCommand(Triggers.Delete,
                                            obj => _graphToDelete = obj as BasicGraph))
            });
        }

        public IReadOnlyDictionary<States, Action> GetStatesEntryAction()
        {
            return new Dictionary<States, Action>
            {
                { States.GraphCreatingForFavorite, ()=> {GraphCreationVM.GoToFirst(); CreatingGraph= true; } },
                {States.SavingNewGraphFavorite, ()=>SaveNewGraph() },
                {States.NewGraphFavoriteSaved, ()=> CreatingGraph= false },
                {States.FavoriteCreatingCanceled, ()=> CreatingGraph= false },
                {States.DeleteFavorite, () => DeleteSelectedGraph()}
            };
        }

        private void DeleteSelectedGraph()
        {
            _graphsModel.DeleteGraph(_graphToDelete);
            _graphsCollection.Remove(_graphToDelete);
            _stateMachine.Fire(Triggers.Finish);
        }

        public IReadOnlyDictionary<States, Action> GetStatesExitAction()
        {
            return new Dictionary<States, Action>();
        }

        private void SaveNewGraph()
        {
            BasicGraph basicGraph;
            switch (GraphCreationVM.SelectedSubject.Key)
            {
                case Subjects.Category:
                    basicGraph = GraphCreationVM.GetGraph<CategoryGraph>();
                    _graphsModel.AddGraph(GraphCreationVM.GetGraph<CategoryGraph>());
                    _stateMachine.Fire(Triggers.Finish);
                    break;
                case Subjects.Product:
                    basicGraph = GraphCreationVM.GetGraph<ProductGraph>();
                    _graphsModel.AddGraph(GraphCreationVM.GetGraph<ProductGraph>());
                    _stateMachine.Fire(Triggers.Finish);
                    break;
                case Subjects.Store:
                    basicGraph = GraphCreationVM.GetGraph<StoreGraph>();
                    _graphsModel.AddGraph(GraphCreationVM.GetGraph<StoreGraph>());
                    _stateMachine.Fire(Triggers.Finish);
                    break;
                default:
                    throw new NotSupportedException();
                    break;
            }
            GraphsCollection.Add(basicGraph);
        }

        private void LoadGraphs()
        {
            GraphsCollection = _graphsModel.GetCategoryGraphs()
                .Concat(_graphsModel.GetStoreGraphs().Select(g => g as BasicGraph))
                .Concat(_graphsModel.GetTransactionGraphs().Select(g => g as BasicGraph))
                .Concat(_graphsModel.GetProductGraphs().Select(g => g as BasicGraph)).ToObservableCollection();
        }

    }
}
