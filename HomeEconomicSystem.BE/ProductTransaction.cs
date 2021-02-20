using System.ComponentModel.DataAnnotations;

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
        [Required]
        public virtual QRData QRData { get; set; }
        public virtual Product Product { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual Store Store { get; set; }

    }
}