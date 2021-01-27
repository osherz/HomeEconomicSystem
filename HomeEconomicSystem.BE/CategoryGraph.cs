using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public class CategoryGraph : BasicGraph
    {
        /// <summary>
        /// The items to show graph for.
        /// </summary>
        public virtual ICollection<Category> Categories { get; set; }
    }
}
