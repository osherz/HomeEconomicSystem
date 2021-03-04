using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public class StoreGraph : BasicGraph
    {
        private ICollection<Store> _collection;
        /// <summary>
        /// The items to show graph for
        /// </summary>
        public virtual ICollection<Store> Stores { get => _collection; set => SetProperty(ref _collection, value); }
    }
}
