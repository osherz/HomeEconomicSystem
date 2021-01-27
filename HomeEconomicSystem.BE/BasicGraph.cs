using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public abstract class BasicGraph<T>
    {
        public int Id { get; set; }
        public TimeType TimeType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GraphType GraphType { get; set; }
        public virtual ICollection<T> Items { get; set; }
    }
}
