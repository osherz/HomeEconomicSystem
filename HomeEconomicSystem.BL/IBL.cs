namespace HomeEconomicSystem.BL
{
    public interface IBL
    {
        IAssosiationProductsAnalysis AssosiationProductsAnalysis { get; }
        IDataManagement DataManagement { get; }
        IGraphManagement GraphManagement { get; }
        ITransactionAnalysis TransactionAnalysis { get; }
        IValidation Validation { get; }
    }
}