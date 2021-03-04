using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.Model;
using HomeEconomicSystem.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class FavoritesVM : INotifyPropertyChanged, IInnerVM<States, Triggers>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        public GraphCreationVM GraphCreationVM { get; }
        GraphsModel _graphsModel;
        Stateless.StateMachine<States, Triggers> _stateMachine;

        BasicGraph _graphToDelete;

        public IReadOnlyList<MenuItem> MenuItems { get; set; }

        private bool _creatingGraph;
        public bool CreatingGraph
        {
            get => _creatingGraph;
            set => SetProperty(ref _creatingGraph, value);
        }

        private ObservableCollection<BasicGraph> _graphsCollection;

        public event EventHandler<string> InnerStateChanged;

        public ObservableCollection<BasicGraph> GraphsCollection
        {
            get { return _graphsCollection; }
            set { SetProperty(ref _graphsCollection, value); }
        }

        #region Commands
        public ICommand CreateGraph { get; private set; }
        #endregion Commands

        public FavoritesVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            GraphCreationVM = new GraphCreationVM();
            _graphsModel = new GraphsModel();
            LoadGraphs();
        }


        public void SetStateMachine(Stateless.StateMachine<States,Triggers> stateMachine)
        {
            _stateMachine = stateMachine;
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
                case Subjects.Transaction:
                    basicGraph = GraphCreationVM.GetGraph<TransactionsGraph>();
                    _graphsModel.AddGraph(GraphCreationVM.GetGraph<TransactionsGraph>());
                    _stateMachine.Fire(Triggers.Finish);
                    break;
                default:
                    throw new NotSupportedException();
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

        private void OnStateChanged(string state)
        {
            InnerStateChanged?.Invoke(this, state);
        }

        private void SetProperty<T>(ref T property, T value)
        {
            _notifyPropertyChanged.SetProperty(ref property, value);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}
