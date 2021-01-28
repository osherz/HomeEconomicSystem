namespace HomeEconomicSystem.BE
{
    public class ProductTransaction
    {
        public int Id { get; set; }
        public float UnitPrice { get; set; }
        /// <summary>
        ///Amount of products.
        /// </summary>
        public float Amount { get; set; }
        public virtual QRData QROriginalData { get; set; }
        public virtual Product Product { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}