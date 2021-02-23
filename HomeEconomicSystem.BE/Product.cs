using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{


    public class Product : IName
    {
        public int Id { get; set; }
        [MinLength(13), MaxLength(15)]
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFileName { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductTransaction> ProductTransactions { get; set; }
    }
}
