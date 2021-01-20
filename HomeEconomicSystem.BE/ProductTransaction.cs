namespace HomeEconomicSystem.BE
{
    public class ProductTransaction
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Transaction Transaction { get; set; }
        public float UnitPrice { get; set; }
        public int Amount { get; set; }
    }
}