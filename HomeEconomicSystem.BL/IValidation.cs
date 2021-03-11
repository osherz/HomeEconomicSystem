using HomeEconomicSystem.BE;

namespace HomeEconomicSystem.BL
{
    public interface IValidation
    {
        bool Validate(Category category);
        bool Validate(CategoryGraph categoryGraph);
        bool Validate(Product product);
        bool Validate(ProductGraph productGraph);
        bool Validate(Store store);
        bool Validate(StoreGraph storeGraph);
        bool Validate(Transaction transaction);
        bool Validate(TransactionsGraph transactionsGraph);
        bool ValidateBaseProperty(BasicGraph basicGraph);
    }
}