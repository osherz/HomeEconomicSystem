namespace HomeEconomicSystem.BL
{
    public interface IBL
    {
        IAssosiatonRule AssosiatonRule { get; }
        IDataManagement DataManagement { get; }
        IGraphManagement GraphManagement { get; }
        ITransactionAnalysis TransactionAnalysis { get; }
    }
}