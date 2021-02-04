using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeEconomicSystem.Dal.GoogleDrive;

namespace HomeEconomicSystem.Dal.Test
{
    class Program
    {
        static void Main()
        {
            QRCodeFetcher qRCodeFetcher = new QRCodeFetcher();
            IEnumerable<IQRcode> qRcode = qRCodeFetcher.GetQRCode();
            foreach (var file in qRcode)
            {
                using (var img = System.IO.File.Create($"{file.FileName}.jpg"))
                {
                    img.Seek(0, SeekOrigin.Begin);
                    var bytes = file.ImageStream;
                    img.Write(bytes, 0, bytes.Length);
                }
                qRCodeFetcher.DeleteQRcode(file.FileName);
            }
        }

    }
}
