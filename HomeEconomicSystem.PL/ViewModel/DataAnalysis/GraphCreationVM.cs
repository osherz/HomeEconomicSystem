using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public enum Subjects { Category, Product, Store};

    public class GraphCreationVM : NotifyPropertyChanged
    {
        private CategoriesModel _categoriesModel;
        private BasicGraph _graph;
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

        KeyValuePair<Subjects, string> _selectedSubject;
        public KeyValuePair<Subjects, string> SelectedSubject
        {
            get => _selectedSubject;
            set => SetProperty(ref _selectedSubject, value);
        }
        public IReadOnlyCollection<KeyValuePair<Subjects, string>> SubjectsOptions { get; }

        KeyValuePair<GraphType, string> _selectedGraphType;
        public KeyValuePair<GraphType, string> SelectedGraphType
        {
            get => _selectedGraphType;
            set
            {
                SetProperty(ref _selectedGraphType, value);
                _graph.GraphType = value.Key;
            }
        }
        public IReadOnlyCollection<KeyValuePair<GraphType, string>> GraphTypeOptions { get; }

        KeyValuePair<AmountOrCost, string> _selectedAmountOrCost;
        public KeyValuePair<AmountOrCost, string> SelectedAmountOrCost
        {
            get => _selectedAmountOrCost;
            set
            {
                SetProperty(ref _selectedAmountOrCost, value);
                _graph.AmountOrCost = value.Key;
            }
        }
        public IReadOnlyCollection<KeyValuePair<AmountOrCost, string>> AmountOrCostOptions { get; }

        KeyValuePair<TimeType, string> _selectedAggregationTimeType;
        public KeyValuePair<TimeType, string> SelectedAggregationTimeType
        {
            get => _selectedAggregationTimeType;
            set
            {
                SetProperty(ref _selectedAggregationTimeType, value);
                _graph.AggregationTimeType = value.Key;
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

        public GraphCreationVM()
        {
            _categoriesModel = new CategoriesModel();
            _graph = new TransactionsGraph(); // Just to get the properties of BasicGraph
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
        }

        public TGraph GetGraph<TGraph>()
            where TGraph: BasicGraph
        {
            return (TGraph)_graph;
        }

        private void GraphCreationVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedSubject))
            {
                LoadSubSubject();
            }
        }

        public void SetStateMachine(DataAnalysisStateMachine stateMachine)
        {
            if (Next != null || Prev != null) throw new InvalidOperationException("Already set a state machine");
            Next = stateMachine.CreateCommand(Triggers.Next);
            Prev = stateMachine.CreateCommand(Triggers.Back);
            Finish = stateMachine.CreateCommand(Triggers.Finish);
            Cancel = stateMachine.CreateCommand(Triggers.Cancel);
        }

        public IReadOnlyDictionary<States, Action> GetStatesEntryAction()
        {
            return new Dictionary<States, Action>
            {
                { States.GraphMeasureChoosing, ()=>MeasureChoosing = true },
                { States.GraphRangeChoosing, ()=>RangeChoosing = true },
                { States.GraphSubjectChoosing, ()=>SubjectChoosing = true },
                { States.GraphSubSubjectChoosing, ()=>SubSubjectChoosing = true },
                { States.GraphTypeChoosing, ()=>{ TypeChoosing = true; }},
            };
        }

        public IReadOnlyDictionary<States, Action> GetStatesExitAction()
        {
            return new Dictionary<States, Action>
            {
                { States.GraphMeasureChoosing, ()=>MeasureChoosing = false },
                { States.GraphRangeChoosing, ()=>RangeChoosing = false },
                { States.GraphSubjectChoosing, ()=> SubjectChoosing = false },
                { States.GraphSubSubjectChoosing, ()=>SubSubjectChoosing = false },
                { States.GraphTypeChoosing, ()=>TypeChoosing = false },
            };
        }

        private void LoadSubSubject()
        {
            switch (SelectedSubject.Key)
            {
                case Subjects.Category:
                    SubSubjects = _categoriesModel.CategoriesList.Select(c => new SelectedableItem<IName>(c)).ToObservableCollection();
                    CategoryGraph categoryGraph = new CategoryGraph();
                    if(_graph!=null) _graph.Copy(categoryGraph);
                    _graph = categoryGraph;
                    break;
                case Subjects.Product:
                    break;
                case Subjects.Store:
                    break;
                default:
                    break;
            }
        }
    }
}