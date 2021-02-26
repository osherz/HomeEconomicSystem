using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    public enum GraphCreationStates
    {
        Start,
        GraphTypeChoosing,
        GraphSubjectChoosing,
        GraphSubSubjectChoosing,
        GraphMeasureChoosing,
        GraphRangeChoosing,
        DoneCreation,
        Canceled
    }

    public enum GraphCreationTriggers
    {
        Next,
        Back,
        Cancel,
        Finish,
        GoToFirst
    }
    public class GraphCreationStateMachine : BaseStateMachine<GraphCreationStates, GraphCreationTriggers>
    {
        private IEnumerable<GraphCreationStates> _graphCreationOrder = new[]
            {
                GraphCreationStates.GraphSubjectChoosing,
                GraphCreationStates.GraphSubSubjectChoosing,
                GraphCreationStates.GraphTypeChoosing,
                GraphCreationStates.GraphMeasureChoosing,
                GraphCreationStates.GraphRangeChoosing
            };

        public GraphCreationVM ParentVM { get; }

        public GraphCreationStateMachine(GraphCreationVM parentVM, IReadOnlyDictionary<GraphCreationStates, Action> stateActionDict, IReadOnlyDictionary<GraphCreationStates, Action> stateExitActionDict = null) : base(GraphCreationStates.Start, stateActionDict, stateExitActionDict)
        {
            ParentVM = parentVM;

            ConfigureGraphCreationStates();

            BasicConfigure(GraphCreationStates.Start)
                .Permit(GraphCreationTriggers.GoToFirst, _graphCreationOrder.First());

            BasicConfigure(GraphCreationStates.Canceled)
                .Permit(GraphCreationTriggers.GoToFirst, _graphCreationOrder.First());

            BasicConfigure(GraphCreationStates.DoneCreation)
                .Permit(GraphCreationTriggers.GoToFirst, _graphCreationOrder.First());
        }

        private void ConfigureGraphCreationStates()
        {
            var graphCreationOrder = _graphCreationOrder.ToArray();
            StateConfiguration config = null;
            for (int creationStateIndex = 0; creationStateIndex < _graphCreationOrder.Count(); creationStateIndex++)
            {
                GraphCreationStates currentState = graphCreationOrder[creationStateIndex];
                bool isTheLastState = creationStateIndex + 1 == _graphCreationOrder.Count();
                // Config next of previous state.
                if (config != null)
                {
                    config.PermitIf(GraphCreationTriggers.Next, currentState, () => !WhetherToSkip(currentState));
                    // Skip on current state.
                    if(isTheLastState)
                    {
                        config.PermitIf(GraphCreationTriggers.Finish, GraphCreationStates.DoneCreation, () => WhetherToSkip(currentState));
                    }
                    else
                    {
                        config.PermitIf(GraphCreationTriggers.Next, graphCreationOrder[creationStateIndex + 1], () => WhetherToSkip(currentState));
                    }
                }

                config = Configure(currentState)
                    .OnEntry(GetStateEntryAction(currentState))
                    .OnExit(GetStateExitAction(currentState))
                    .Permit(GraphCreationTriggers.Cancel, GraphCreationStates.Canceled);

                // Config back of current state.
                if (creationStateIndex > 0)
                {
                    GraphCreationStates prev = graphCreationOrder[creationStateIndex - 1];
                    config.PermitIf(GraphCreationTriggers.Back, prev, () => !WhetherToSkip(prev))
                          .Permit(GraphCreationTriggers.GoToFirst, _graphCreationOrder.First());

                    // Skip on prev state.
                    if (creationStateIndex >= 2)
                    {
                        config.PermitIf(GraphCreationTriggers.Back, graphCreationOrder[creationStateIndex - 2], () => WhetherToSkip(prev));
                    }
                }
            }
            config.Permit(GraphCreationTriggers.Finish, GraphCreationStates.DoneCreation);
        }

        private bool WhetherToSkip(GraphCreationStates state)
        {
            return state == GraphCreationStates.GraphSubSubjectChoosing &&
                ParentVM.SelectedSubject.Key == Subjects.Transaction;
        }
    }
}
