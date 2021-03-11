using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.Transactions
{
    public enum TransactionsState
    {
        NewTransactionCreating,
        ChoosingCategory,
        ChoosedCategory,
        ChoosingProduct,
        ChoosedProduct,
        ChoosingStore,
        ChoosedStore,
        UpdatingTransaction,
        DoneCreating
    }

    public enum TransactionsTriggers
    {
        ChangeCategory,
        ChangeProduct,
        ChangeStore,
        Finish
    }

    public class TransactionsStateMachine : BaseStateMachine<TransactionsState, TransactionsTriggers>
    {


        public TransactionsStateMachine(CreateTransactionVM parentVM, IReadOnlyDictionary<TransactionsState, Action> stateActionDict, IReadOnlyDictionary<TransactionsState, Action> stateExitActionDict = null) : base(TransactionsState.NewTransactionCreating, stateActionDict, stateExitActionDict)
        {
            ParentVM = parentVM;

            BasicConfigure(TransactionsState.NewTransactionCreating)
                .Permit(TransactionsTriggers.ChangeCategory, TransactionsState.ChoosingCategory)
                .Permit(TransactionsTriggers.ChangeProduct, TransactionsState.ChoosingProduct)
                .Permit(TransactionsTriggers.ChangeStore, TransactionsState.ChoosingStore)
                .PermitIf(TransactionsTriggers.Finish, TransactionsState.UpdatingTransaction, () => new BL.BL().Validation.Validate(ParentVM.Transaction));

            BasicConfigure(TransactionsState.ChoosingCategory)
                .Permit(TransactionsTriggers.Finish, TransactionsState.ChoosedCategory);

            BasicConfigure(TransactionsState.ChoosingProduct)
                .Permit(TransactionsTriggers.Finish, TransactionsState.ChoosedProduct);

            BasicConfigure(TransactionsState.ChoosingStore)
                .Permit(TransactionsTriggers.Finish, TransactionsState.ChoosedStore);

            BasicConfigure(TransactionsState.ChoosedCategory, TransactionsState.NewTransactionCreating);

            BasicConfigure(TransactionsState.ChoosedProduct, TransactionsState.NewTransactionCreating);

            BasicConfigure(TransactionsState.ChoosedStore, TransactionsState.NewTransactionCreating);

            BasicConfigure(TransactionsState.UpdatingTransaction)
                .PermitIf(TransactionsTriggers.Finish, TransactionsState.DoneCreating, () => new BL.BL().Validation.Validate(ParentVM.Transaction));

            BasicConfigure(TransactionsState.DoneCreating, TransactionsState.NewTransactionCreating);
        }

        public CreateTransactionVM ParentVM { get; }
    }
}
