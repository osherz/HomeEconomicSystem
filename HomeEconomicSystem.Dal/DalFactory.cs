using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal
{
    public class DalFactory
    {
        private IQRCodeFetcher _qRCodeFetcher;
        private IDb _db;

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
