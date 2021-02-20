using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeEconomicSystem.BE;
using HomeEconomicSystem.Dal.GoogleDrive;

namespace HomeEconomicSystem.Dal.Test
{
    class Program
    {
        static void Main()
        {

            var db = new HomeEconomicSystem.Dal.DalFactory().GetDb();
            var categories = new[] {"מוצרי חלב", "בשר", "בגדים", "יבשים", "ילדים", "עוגות ועוגיות","סלטים" };
            var stores = new[] { "רמי לוי סניף גוש עציון", "יעקבי סניף קריית ארבע", "אושר עד סניף נוף איילון" };
            db.Categories.AddRange(categories.Select(n=>new Category { Name = n }));
            db.Stores.AddRange(stores.Select(n => new Store { Name = n}));
            db.SaveChanges();

            //QRCodeFetcher qRCodeFetcher = new QRCodeFetcher();
            //IEnumerable<IQRcode> qRcode = qRCodeFetcher.GetQRCode();
            //foreach (var file in qRcode)
            //{
            //    using (var img = System.IO.File.Create($"{file.FileName}.jpg"))
            //    {
            //        img.Seek(0, SeekOrigin.Begin);
            //        var bytes = file.ImageStream;
            //        img.Write(bytes, 0, bytes.Length);
            //    }
            //    qRCodeFetcher.DeleteQRcode(file.FileName);
            //}
        }

    }
}
