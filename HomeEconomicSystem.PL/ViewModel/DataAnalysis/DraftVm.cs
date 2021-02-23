﻿using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.Model;
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
    public class DraftVM : NotifyPropertyChanged
    {
        public GraphCreationVM GraphCreationVM { get; }
        GraphsModel _graphsModel;
        DataAnalysisStateMachine _stateMachine;

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

        public DraftVM()
        {
            PropertyChanged += DraftVM_PropertyChanged;
            GraphCreationVM = new GraphCreationVM();
            _graphsModel = new GraphsModel();
            GraphsCollection = new ObservableCollection<BasicGraph>();
        }

        private void DraftVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CreatingGraph = GraphCreationVM.TypeChoosing ||
                GraphCreationVM.RangeChoosing ||
                GraphCreationVM.SubjectChoosing ||
                GraphCreationVM.SubSubjectChoosing ||
                GraphCreationVM.MeasureChoosing;
        }

        public void SetStateMachine(DataAnalysisStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.OnTransitionCompleted((e) => State = e.Destination);
            CreateGraph = stateMachine.CreateCommand(Triggers.CreateGraph);
            GraphCreationVM.SetStateMachine(stateMachine);
        }

        public IReadOnlyDictionary<States, Action> GetStatesEntryAction()
        {
            return new Dictionary<States, Action>
            {
                {States.SavingNewGraph, ()=>SaveNewGraph() }
            }.Concat(GraphCreationVM.GetStatesEntryAction()).ToDictionary(item=>item.Key, item=>item.Value);
        }

        private void SaveNewGraph()
        {
            BasicGraph basicGraph;
            switch (GraphCreationVM.SelectedSubject.Key)
            {
                case Subjects.Category:
                    basicGraph = GraphCreationVM.GetGraph<CategoryGraph>();
                    break;
                case Subjects.Product:
                    basicGraph = GraphCreationVM.GetGraph<ProductGraph>();
                    break;
                case Subjects.Store:
                    basicGraph = GraphCreationVM.GetGraph<StoreGraph>();
                    break;
                default:
                    throw new NotSupportedException();
                    break;
            }
            GraphsCollection.Add(basicGraph);
            _stateMachine.Fire(Triggers.Finish);
        }

        public IReadOnlyDictionary<States, Action> GetStatesExitAction()
        {
            return GraphCreationVM.GetStatesExitAction();
        }
    }
}
