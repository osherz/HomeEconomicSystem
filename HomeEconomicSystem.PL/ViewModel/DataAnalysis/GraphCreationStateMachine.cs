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
                GraphCreationStates.GraphTypeChoosing,
                GraphCreationStates.GraphSubSubjectChoosing,
                GraphCreationStates.GraphMeasureChoosing,
                GraphCreationStates.GraphRangeChoosing
            };

        public GraphCreationStateMachine(IReadOnlyDictionary<GraphCreationStates, Action> stateActionDict, IReadOnlyDictionary<GraphCreationStates, Action> stateExitActionDict = null) : base(GraphCreationStates.Start, stateActionDict, stateExitActionDict)
        {
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
                // Config next of previous state.
                if (config != null)
                {   
                    config.Permit(GraphCreationTriggers.Next, currentState);
                }

                config = Configure(currentState)
                    .OnEntry(GetStateEntryAction(currentState))
                    .OnExit(GetStateExitAction(currentState))
                    .Permit(GraphCreationTriggers.Cancel, GraphCreationStates.Canceled);

                // Config back of current state.
                if (creationStateIndex > 0)
                {
                    GraphCreationStates prev = graphCreationOrder[creationStateIndex - 1];
                    config.Permit(GraphCreationTriggers.Back, prev)
                          .Permit(GraphCreationTriggers.GoToFirst, _graphCreationOrder.First());
                }
            }
            config.Permit(GraphCreationTriggers.Finish, GraphCreationStates.DoneCreation);
        }
    }
}
