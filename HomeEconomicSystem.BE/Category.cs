using System.Collections.Generic;

namespace HomeEconomicSystem.BE
{
    public class Category : IName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageFileName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}