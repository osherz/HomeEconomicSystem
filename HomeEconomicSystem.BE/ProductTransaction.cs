namespace HomeEconomicSystem.BE
{
    public class ProductTransaction
    {
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public virtual Transaction Transaction { get; set; }
        public float UnitPrice { get; set; }
        public int Amount { get; set; }
    }
}