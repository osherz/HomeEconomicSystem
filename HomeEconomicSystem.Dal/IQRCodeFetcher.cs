using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal
{
    public interface IQRCodeFetcher
    {
        /// <summary>
        /// Return all QRCodes files (stream and name).
        /// </summary>
        /// <exception cref="DownloadFileException">If failed to download</exception>
        /// <returns></returns>
        IEnumerable<IQRcode> GetQRCode();

        /// <summary>
        /// Delete the QRcode files by their names.
        /// </summary>
        /// <param name="qrCodeFilesNamesToDelete">Get list of QRcode names to delete</param>
        /// <exception cref="DeleteFilesException">If some of the files failed to deletion</exception>
        void DeleteQRcode(params string[] qrCodeFilesNamesToDelete);
    }
}
