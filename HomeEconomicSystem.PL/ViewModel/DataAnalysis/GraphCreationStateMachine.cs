using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Model;
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
        private IEnumerable<GraphCreationStates> _graphCreationOrder;
        private IReadOnlyDictionary<GraphCreationStates, Func<bool>> _graphCreationGurads;

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
            _graphCreationOrder = new List<GraphCreationStates>
            {
                GraphCreationStates.GraphSubjectChoosing,
                GraphCreationStates.GraphSubSubjectChoosing,
                GraphCreationStates.GraphTypeChoosing,
                GraphCreationStates.GraphMeasureChoosing,
                GraphCreationStates.GraphRangeChoosing,
            };

            _graphCreationGurads = new Dictionary<GraphCreationStates, Func<bool>>
            {
                { GraphCreationStates.GraphSubjectChoosing, new (() => ValidateTransitionFromSubjectChoosing()) },
                { GraphCreationStates.GraphSubSubjectChoosing, new (() => ValidateTransitionFromSubSubjectChoosing()) },
                { GraphCreationStates.GraphTypeChoosing, new (() => ValidateTransitionFromTypeChoosing()) },
                { GraphCreationStates.GraphMeasureChoosing, new (() => ValidateTransitionFromMeasureChoosing()) },
                { GraphCreationStates.GraphRangeChoosing, new (() => ValidateTransitionFromRangeChoosing()) },
            };

            var graphCreationOrder = _graphCreationOrder.ToArray();
            StateConfiguration config = null;
            Tuple<Func<bool>, string> guardsMoveNext = null;

            var validation = new BL.BL().Validation;
            for (int creationStateIndex = 0; creationStateIndex < _graphCreationOrder.Count(); creationStateIndex++)
            {
                GraphCreationStates currentState = graphCreationOrder[creationStateIndex];
                bool isTheLastState = creationStateIndex + 1 == _graphCreationOrder.Count();
                // Config next of previous state.
                if (config != null)
                {
                    var whetherNotToSkip = Tuple.Create<Func<bool>, string>
                        (
                            new(() => !WhetherToSkip(currentState)),
                            "WhetherNotToSkip"
                        );
                    var whetherToSkip = Tuple.Create<Func<bool>, string>
                        (
                            new(() => WhetherToSkip(currentState)),
                            "WhetherToSkip"
                        );

                    config.PermitIf(GraphCreationTriggers.Next, currentState, whetherNotToSkip, guardsMoveNext);

                    // Skip on current state.
                    if (isTheLastState)
                    {
                        config.PermitIf(GraphCreationTriggers.Finish, GraphCreationStates.DoneCreation, whetherToSkip, guardsMoveNext);
                    }
                    else
                    {
                        config.PermitIf(GraphCreationTriggers.Next, graphCreationOrder[creationStateIndex + 1], whetherToSkip, guardsMoveNext);
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
                guardsMoveNext = Tuple.Create<Func<bool>, string>
                        (
                            new(_graphCreationGurads[graphCreationOrder[creationStateIndex]]),
                            "guards"
                        );
            }
            config.PermitIf(GraphCreationTriggers.Finish, GraphCreationStates.DoneCreation, guardsMoveNext);
        }

        private bool ValidateTransitionFromRangeChoosing()
        {
            return ParentVM.StartDate <= ParentVM.EndDate;
        }

        private bool ValidateTransitionFromMeasureChoosing()
        {
            return ParentVM.SelectedAmountOrCost.HasValue &&
                ParentVM.SelectedAggregationTimeType.HasValue;
        }

        private bool ValidateTransitionFromTypeChoosing()
        {
            return ParentVM.SelectedGraphType.HasValue;
        }

        private bool ValidateTransitionFromSubSubjectChoosing()
        {
            return ParentVM.SubSubjects.Any(s => s.IsSelected);
        }

        private bool WhetherToSkip(GraphCreationStates state)
        {
            return state == GraphCreationStates.GraphSubSubjectChoosing &&
                ParentVM.SelectedSubject?.Key == Subjects.Transaction;
        }

        private bool ValidateTransitionFromSubjectChoosing()
        {
            try
            {
                if (string.IsNullOrEmpty(ParentVM.GraphTitle) || ParentVM.SelectedSubject is null)
                    return false;
                else
                {
                    var graphsModel = new GraphsModel();
                    var title = ParentVM.GraphTitle;
                    IEnumerable<BasicGraph> graphs = null;
                    switch (ParentVM.SelectedSubject.Value.Key)
                    {
                        case Subjects.Category:
                            graphs = graphsModel.GetCategoryGraphs();
                            break;
                        case Subjects.Product:
                            graphs = graphsModel.GetProductGraphs();
                            break;
                        case Subjects.Store:
                            graphs = graphsModel.GetStoreGraphs();
                            break;
                        case Subjects.Transaction:
                            graphs = graphsModel.GetTransactionGraphs();
                            break;
                        default:
                            break;
                    }
                    return graphs.All(g => g.Title != title);
                }
            }
            catch (Exception e)
            {
                ParentVM.ParentPageDisplay.MessageToShow = e.Message;
                ParentVM.ParentPageDisplay.ShowMessage = true;
                return false;
            }
        }
    }
}
