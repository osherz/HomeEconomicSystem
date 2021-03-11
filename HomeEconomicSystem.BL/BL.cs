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
        public ITransactionAnalysis TransactionAnalysis { get; }
        public IGraphManagement GraphManagement { get;  }
        public IDataManagement DataManagement { get; }
        public IAssosiationProductsAnalysis AssosiationProductsAnalysis { get; }
        public IValidation Validation { get; }

        public BL()
        {
            TransactionAnalysis = new TransactionAnalysis();
            GraphManagement = new GraphManagement();
            DataManagement = new DataManagement();
            AssosiationProductsAnalysis = new AssosiationProductsAnalysis();
            Validation = new Validation();
        }
    }
}
