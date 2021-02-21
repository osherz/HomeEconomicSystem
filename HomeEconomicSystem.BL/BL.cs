using HomeEconomicSystem.BL.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL
{
    public class BL : IBL
    {
        public ITransactionAnalysis TransactionAnalysis { get; private set; }
        public IGraphManagement GraphManagement { get; private set; }
        public IDataManagement DataManagement { get; private set; }
        public IAssosiatonRule AssosiatonRule { get; private set; }

        public BL()
        {
            TransactionAnalysis = new TransactionAnalysis();
            GraphManagement = new GraphManagement();
            DataManagement = new DataManagement();
            AssosiatonRule = new AssosiatonRule();
        }
    }
}
