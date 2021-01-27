using System.Collections.Generic;

namespace HomeEconomicSystem.BE
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageFileName { get; set; }
        public virtual List<Product> Products { get; private set; }
    }
}