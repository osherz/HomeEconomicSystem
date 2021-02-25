using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public abstract class BasicGraph
    {
        public int Id { get; set; }
        [MaxLength(100)]

        
        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Determine the aggregation timelaps.
        /// </summary>
        public TimeType AggregationTimeType { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// for "PastTimeAmount" = 3, the graph will show the last 3 "PastTimeType".
        /// </summary>
        public int? PastTimeAmount { get; set; }
        public TimeType? PastTimeType { get; set; }
        public GraphType GraphType { get; set; }
        public AmountOrCost AmountOrCost { get; set; }
    }
}
