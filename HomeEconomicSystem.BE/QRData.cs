using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public class QRData
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public float UnitPrice { get; set; }
        public string StoreName{ get; set; }
        public bool IsNew { get => (ProductTransaction is null); }
        public virtual ProductTransaction ProductTransaction { get; set; }
    }
}
