using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.Model;
using HomeEconomicSystem.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public enum Subjects { Category, Product, Store, Transaction };

    public class GraphCreationVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        public event EventHandler Done;
        public event EventHandler Canceled;


        GraphCreationStateMachine _stateMachine;
        private CategoriesModel _categoriesModel;
        private ProductsModel _productsModel;
        private StoresModel _storesModel;

        private BasicGraph _graph;
        public BasicGraph Graph
        {
            get => _graph;
            set => SetProperty(ref _graph, value);
        }

        public string GraphTitle
        {
            get => _graph.Title;
            set => _graph.Title = value;
        }

        public string GraphDescription
        {
            get => _graph.Description;
            set => _graph.Description = value;
        }

        ObservableCollection<SelectedableItem<IName>> _subSubjects;
        public ObservableCollection<SelectedableItem<IName>> SubSubjects
        {
            get => _subSubjects;
            set => SetProperty(ref _subSubjects, value);
        }

        KeyValuePair<Subjects, string>? _selectedSubject;
        public KeyValuePair<Subjects, string>? SelectedSubject
        {
            get => _selectedSubject;
            set => SetProperty(ref _selectedSubject, value);
        }
        public IReadOnlyCollection<KeyValuePair<Subjects, string>> SubjectsOptions { get; }

        KeyValuePair<GraphType, string>? _selectedGraphType;
        public KeyValuePair<GraphType, string>? SelectedGraphType
        {
            get => _selectedGraphType;
            set
            {
                SetProperty(ref _selectedGraphType, value);
                if(value.HasValue)
                    _graph.GraphType = value.Value.Key;
            }
        }
        public IReadOnlyCollection<KeyValuePair<GraphType, string>> GraphTypeOptions { get; }

        KeyValuePair<AmountOrCost, string>? _selectedAmountOrCost;
        public KeyValuePair<AmountOrCost, string>? SelectedAmountOrCost
        {
            get => _selectedAmountOrCost;
            set
            {
                SetProperty(ref _selectedAmountOrCost, value);
                if (value.HasValue)
                    _graph.AmountOrCost = value.Value.Key;
            }
        }
        public IReadOnlyCollection<KeyValuePair<AmountOrCost, string>> AmountOrCostOptions { get; }

        KeyValuePair<TimeType, string>? _selectedAggregationTimeType;
        public KeyValuePair<TimeType, string>? SelectedAggregationTimeType
        {
            get => _selectedAggregationTimeType;
            set
            {
                SetProperty(ref _selectedAggregationTimeType, value);
                if (value.HasValue)
                    _graph.AggregationTimeType = value.Value.Key;
            }
        }
        public IReadOnlyCollection<KeyValuePair<TimeType, string>> TimeTypeOptions { get; }

        public DateTime StartDate
        {
            get => _graph.StartDate.Value;
            set => _graph.StartDate = value;
        }
        public DateTime EndDate
        {
            get => _graph.EndDate.Value;
            set => _graph.EndDate = value;
        }

        private bool _typeChoosing;

        private bool _subjectChoosing;

        private bool _subSubjectChoosing;

        private bool _measureChoosing;

        private bool _rangeChoosing;
        public bool TypeChoosing
        {
            get { return _typeChoosing; }
            set { SetProperty(ref _typeChoosing, value); }
        }
        public bool SubjectChoosing
        {
            get { return _subjectChoosing; }
            set { SetProperty(ref _subjectChoosing, value); }
        }
        public bool SubSubjectChoosing
        {
            get { return _subSubjectChoosing; }
            set { SetProperty(ref _subSubjectChoosing, value); }
        }
        public bool MeasureChoosing
        {
            get { return _measureChoosing; }
            set { SetProperty(ref _measureChoosing, value); }
        }
        public bool RangeChoosing
        {
            get { return _rangeChoosing; }
            set { SetProperty(ref _rangeChoosing, value); }
        }
        public ICommand Next { get; private set; }
        public ICommand Finish { get; private set; }
        public ICommand Prev { get; private set; }
        public ICommand Cancel { get; private set; }

        public GraphCreationVM() : this(new TransactionsGraph() {StartDate = DateTime.Now, EndDate=DateTime.Now }) // Just to get the properties of BasicGraph
        {

        }

        public GraphCreationVM(BasicGraph graph)
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));

            _categoriesModel = new CategoriesModel();
            _productsModel = new ProductsModel();
            _storesModel = new StoresModel();

            _graph = graph;
            SubjectsOptions = Enum.GetValues(typeof(Subjects))
                .Cast<Subjects>()
                .ToKeyValuePair();
            GraphTypeOptions = Enum.GetValues(typeof(GraphType))
                .Cast<GraphType>()
                .ToKeyValuePair();
            AmountOrCostOptions = Enum.GetValues(typeof(AmountOrCost))
                .Cast<AmountOrCost>()
                .ToKeyValuePair();
            TimeTypeOptions = Enum.GetValues(typeof(TimeType))
                .Cast<TimeType>()
                .ToKeyValuePair();
            PropertyChanged += GraphCreationVM_PropertyChanged;

            CreateStateMachine();
        }

        public TGraph GetGraph<TGraph>()
            where TGraph : BasicGraph
        {
            if (SelectedSubject.Value.Key != Subjects.Transaction)
            {
                var selectedSubSubjects = SubSubjects.Where(item => item.IsSelected).Select(item => item.Item);
                switch (SelectedSubject.Value.Key)
                {
                    case Subjects.Category:
                        (_graph as CategoryGraph).Categories = selectedSubSubjects.Cast<Category>().ToList();
                        break;
                    case Subjects.Product:
                        (_graph as ProductGraph).Products = selectedSubSubjects.Cast<Product>().ToList();
                        break;
                    case Subjects.Store:
                        (_graph as StoreGraph).Stores = selectedSubSubjects.Cast<Store>().ToList();
                        break;
                    default:
                        break;
                }
            }
            return (TGraph)_graph;
        }

        private void GraphCreationVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedSubject))
            {
                LoadSubSubject();
            }
        }

        private void CreateStateMachine()
        {
            _stateMachine = new(this, GetStatesEntryAction(), GetStatesExitAction());

            if (Next != null || Prev != null) throw new InvalidOperationException("Already set a state machine");
            Next = _stateMachine.CreateCommand(GraphCreationTriggers.Next);
            Prev = _stateMachine.CreateCommand(GraphCreationTriggers.Back);
            Finish = _stateMachine.CreateCommand(GraphCreationTriggers.Finish);
            Cancel = _stateMachine.CreateCommand(GraphCreationTriggers.Cancel);
        }

        private IReadOnlyDictionary<GraphCreationStates, Action> GetStatesEntryAction()
        {
            return new Dictionary<GraphCreationStates, Action>
            {
                { GraphCreationStates.GraphMeasureChoosing, ()=>MeasureChoosing = true },
                { GraphCreationStates.GraphRangeChoosing, ()=>RangeChoosing = true },
                { GraphCreationStates.GraphSubjectChoosing, ()=>SubjectChoosing = true },
                { GraphCreationStates.GraphSubSubjectChoosing, ()=>SubSubjectChoosing = true },
                { GraphCreationStates.GraphTypeChoosing, ()=>{ TypeChoosing = true; }},
                {GraphCreationStates.DoneCreation, () => Done?.Invoke(this, EventArgs.Empty) },
                {GraphCreationStates.Canceled, () => Canceled?.Invoke(this, EventArgs.Empty) }
            };
        }

        public void GoToFirst()
        {
            _stateMachine.Fire(GraphCreationTriggers.GoToFirst);
        }

        public IReadOnlyDictionary<GraphCreationStates, Action> GetStatesExitAction()
        {
            return new Dictionary<GraphCreationStates, Action>
            {
                { GraphCreationStates.GraphMeasureChoosing, ()=>MeasureChoosing = false },
                { GraphCreationStates.GraphRangeChoosing, ()=>RangeChoosing = false },
                { GraphCreationStates.GraphSubjectChoosing, ()=> SubjectChoosing = false },
                { GraphCreationStates.GraphSubSubjectChoosing, ()=>SubSubjectChoosing = false },
                { GraphCreationStates.GraphTypeChoosing, ()=>TypeChoosing = false },
            };
        }

        private void LoadSubSubject()
        {
            BasicGraph newGraph;
            if (SelectedSubject.HasValue)
            {
                Subjects selectedSubject = SelectedSubject.Value.Key;
                if (selectedSubject != Subjects.Transaction)
                {
                    IEnumerable<IName> subSubjectsNames;
                    switch (selectedSubject)
                    {
                        case Subjects.Category:
                            subSubjectsNames = _categoriesModel.CategoriesList;
                            newGraph = new CategoryGraph();
                            break;
                        case Subjects.Product:
                            subSubjectsNames = _productsModel.ProductsList;
                            newGraph = new ProductGraph();
                            break;
                        case Subjects.Store:
                            subSubjectsNames = _storesModel.StoresList;
                            newGraph = new StoreGraph();
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    SubSubjects = subSubjectsNames.Select(c => new SelectedableItem<IName>(c)).ToObservableCollection();
                }
                else newGraph = new TransactionsGraph();

                if (_graph != null) _graph.Copy(newGraph);
                _graph = newGraph;
            }
        }

        public void Reset()
        {
            Graph = new TransactionsGraph() { StartDate = DateTime.Now, EndDate = DateTime.Now };
            SelectedAggregationTimeType = null;
            SelectedAmountOrCost = null;
            SelectedGraphType = null;
            SelectedSubject = null;
            SubSubjects = null;
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}