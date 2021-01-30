using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal
{
    public interface IQRCodeFetcher
    {
        IEnumerable<IQRcode> GetQRCode();
        /// <summary>
        /// delete the QRcode.
        /// </summary>
        /// <param name="qrCodeToDelete">get list of QRcode to delete</param>
        /// <returns>return all QRcode that didnt manage to delete</returns>
        IEnumerable<IQRcode> DeleteQRcode(IEnumerable<IQRcode> qrCodeToDelete);
    }
}
