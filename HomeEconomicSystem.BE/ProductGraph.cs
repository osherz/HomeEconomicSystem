using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public class ProductGraph : BasicGraph
    {

        private ICollection<Product> _collection;
        /// <summary>
        /// The items to show graph for.
        /// </summary>
        public virtual ICollection<Product> Products { get => _collection; set => SetProperty(ref _collection, value); }

    }
}
