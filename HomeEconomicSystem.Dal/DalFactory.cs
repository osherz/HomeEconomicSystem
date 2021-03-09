using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal
{
    public class DalFactory
    {
        private static IQRCodeFetcher _qRCodeFetcher;
        private static IDb _db;
        // TODO: Remove static!!
        public IQRCodeFetcher GetQRCodeFetcher() 
        {
            if(_qRCodeFetcher is null)
            {
                _qRCodeFetcher = new GoogleDrive.QRCodeFetcher();
            }
            return _qRCodeFetcher;
        }
        public IDb GetDb() 
        {
            if(_db is null)
            {
                _db = new EntityFramework.HomeEconomicContext();
            }
            return _db;
        }
    }
}
