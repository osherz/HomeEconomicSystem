using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public class Store : IName
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ICollection<ProductTransaction> ProductTransaction { get; set; }
    }
}
