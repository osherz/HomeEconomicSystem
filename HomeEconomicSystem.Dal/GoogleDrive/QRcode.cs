using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal.GoogleDrive
{
    public class QRcode : IQRcode
    {
        public byte[] ImageStream { get; internal set; }
        public string FileName { get; internal set; }
    }
}
