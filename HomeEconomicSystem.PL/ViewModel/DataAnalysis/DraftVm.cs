using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public class DraftVM : NotifyPropertyChanged
    {

        private bool _typeChoosing;
        public bool TypeChoosing
        {
            get { return _typeChoosing; }
            set { SetProperty(ref _typeChoosing, value); }
        }

        private bool _subjectChoosing;
        public bool SubjectChoosing
        {
            get { return _subjectChoosing; }
            set { SetProperty(ref _subjectChoosing, value); }
        }

        private bool _subSubjectChoosing;
        public bool SubSubjectChoosing
        {
            get { return _subSubjectChoosing; }
            set { SetProperty(ref _subSubjectChoosing, value); }
        }

        private bool _measureChoosing;
        public bool MeasureChoosing
        {
            get { return _measureChoosing; }
            set { SetProperty(ref _measureChoosing, value); }
        }

        private bool _rangeChoosing;
        public bool RangeChoosing
        {
            get { return _rangeChoosing; }
            set { SetProperty(ref _rangeChoosing, value); }
        }

        private bool _creatingGraph;
        public bool CreatingGraph
        {
            get => _creatingGraph;
            set => SetProperty(ref _creatingGraph, value);
        }

        States _state;
        public States State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        #region Commands
        public ICommand CreateGraph { get; private set; }
        public ICommand Next { get; private set; }
        public ICommand Finish { get; private set; }
        public ICommand Prev { get; private set; }
        public ICommand Cancel { get; private set; }
        #endregion Commands

        public DraftVM()
        {
            PropertyChanged += DraftVM_PropertyChanged;
        }

        private void DraftVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CreatingGraph = TypeChoosing || RangeChoosing || SubjectChoosing || SubSubjectChoosing || MeasureChoosing;
        }

        public void SetStateMachine(DataAnalysisStateMachine stateMachine)
        {
            stateMachine.OnTransitionCompleted((e) => State = e.Destination);
            if (Next != null || Prev != null) throw new InvalidOperationException("Already set a state machine");
            CreateGraph = stateMachine.CreateCommand(Triggers.CreateGraph);
            Next = stateMachine.CreateCommand(Triggers.Next);
            Prev = stateMachine.CreateCommand(Triggers.Back);
            Finish = stateMachine.CreateCommand(Triggers.Finish);
            Cancel = stateMachine.CreateCommand(Triggers.Cancel);
        }

        public IReadOnlyDictionary<States, Action> GetStatesAction()
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
                { States.GraphSubjectChoosing, ()=>SubjectChoosing = false },
                { States.GraphSubSubjectChoosing, ()=>SubSubjectChoosing = false },
                { States.GraphTypeChoosing, ()=>TypeChoosing = false },
            };
        }
    }
}
