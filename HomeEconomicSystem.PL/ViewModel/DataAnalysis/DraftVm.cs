using HomeEconomicSystem.BE;
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
        public GraphCreationVM _graphCreationVM;
        public GraphCreationVM GraphCreationVM
        {
            get => _graphCreationVM;
            set => SetProperty(ref _graphCreationVM, value);
        }
        DataAnalysisStateMachine _stateMachine;

        private bool _creatingGraph;
        public bool CreatingGraph
        {
            get => _creatingGraph;
            set => SetProperty(ref _creatingGraph, value);
        }

        private BasicGraph _basicGraph;
        public BasicGraph Graph
        {
            get => _basicGraph;
            set => SetProperty(ref _basicGraph, value);
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
            GraphCreationVM = new GraphCreationVM();
        }

        public void SetStateMachine(DataAnalysisStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.OnTransitionCompleted((e) => State = e.Destination);
            CreateGraph = stateMachine.CreateCommand(Triggers.CreateGraph);

        }

        public IReadOnlyDictionary<States, Action> GetStatesEntryAction()
        {
            return new Dictionary<States, Action>
            {
                {
                    States.GraphCreatingForDraft, ()=>
                    {
                        if(GraphCreationVM is not null)
                        {
                            GraphCreationVM.Done -= GraphCreationVM_Done;
                            GraphCreationVM.Canceled -= GraphCreationVM_Canceled;
                        }
                        GraphCreationVM = new GraphCreationVM(Graph??new TransactionsGraph());
                        GraphCreationVM.GoToFirst();
                        CreatingGraph= true;
                        GraphCreationVM.Done += GraphCreationVM_Done;
                        GraphCreationVM.Canceled += GraphCreationVM_Canceled;
                    }
                },
                {States.SavingNewGraphDraft, ()=>SaveNewGraph() },
                {States.NewGraphDraftSaved, ()=> CreatingGraph= false },
                {States.DraftCreatingCanceled, ()=> CreatingGraph= false }
            };
        }

        private void GraphCreationVM_Canceled(object sender, EventArgs e)
        {
            _stateMachine.Fire(Triggers.Cancel);
        }

        private void GraphCreationVM_Done(object sender, EventArgs e)
        {
            _stateMachine.Fire(Triggers.Finish);
        }

        public IReadOnlyDictionary<States, Action> GetStatesExitAction()
        {
            return new Dictionary<States, Action>();
        }

        private void SaveNewGraph()
        {
            BasicGraph basicGraph = GraphCreationVM.SelectedSubject.Key switch
            {
                Subjects.Category => GraphCreationVM.GetGraph<CategoryGraph>(),
                Subjects.Product => GraphCreationVM.GetGraph<ProductGraph>(),
                Subjects.Store => GraphCreationVM.GetGraph<StoreGraph>(),
                _ => throw new NotSupportedException(),
            };
            Graph = basicGraph;
            _stateMachine.Fire(Triggers.Finish);
        }
    }
}
