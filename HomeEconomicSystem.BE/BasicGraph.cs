using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public abstract class BasicGraph
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Determine the aggregation timelaps.
        /// </summary>
        public TimeType TimeType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GraphType GraphType { get; set; }
        public AmountOrCost AmountOrCost { get; set; }
    }
}
