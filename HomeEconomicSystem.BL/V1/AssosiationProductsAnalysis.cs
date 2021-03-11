using Accord.MachineLearning.Rules;
using HomeEconomicSystem.BE;
using HomeEconomicSystem.Dal;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL.V1
{
    internal class AssosiationProductsAnalysis : IAssosiationProductsAnalysis
    {
        private IDb _db;

        public AssosiationProductsAnalysis()
        {
            _db = new DalFactory().GetDb();
        }

        public void CreateShopingListRecommendation(string path)
        {
            IEnumerable<IEnumerable<Product>> products = GetNewRecommendedProductList();

            //Create a new PDF document
            PdfDocument doc = new PdfDocument();
            //Add a page
            PdfPage page = doc.Pages.Add();
            // Create a PdfLightTable
            PdfLightTable pdfLightTable = new PdfLightTable();
            // Initialize DataTable to assign as DateSource to the light table
            DataTable table = new DataTable();
            //Include columns to the DataTable
            table.Columns.Add("Product Name");
            table.Columns.Add("Bar Code");
            //Include rows to the DataTable
            if(products == null || products.Count() == 0)
                table.Rows.Add(new string[] { "There is still no shoping list", "" });
            else
                foreach (var productGroup in products)
                {
                    foreach (var product in productGroup)
                    {
                        table.Rows.Add(new string[] { product.Name.ToString(), product.BarCode.ToString() });
                    }
                }
            //Applying cell padding to table
            pdfLightTable.Style.CellPadding = 3;
            pdfLightTable.ApplyBuiltinStyle(PdfLightTableBuiltinStyle.GridTable3Accent3);
            //Assign data source
            pdfLightTable.DataSource = table;
            //Setting this property to true to show the header of table
            pdfLightTable.Style.ShowHeader = true;
            //Draw PdfLightTable
            pdfLightTable.Draw(page, new PointF(0, 0));
            //Save the document
            //doc.Save("ShopingList" + DateTime.Now.ToString() + ".pdf");
            doc.Save("PdfTable.pdf");
            //Close the document
            doc.Close(true);
            //This will open the PDF file so, the result will be seen in default PDF viewer
            Process.Start("PdfTable.pdf");

        }


        private IEnumerable<IEnumerable<Product>> GetNewRecommendedProductList()
        {
            IEnumerable<IAssociationRule> associationRules = GetAssosiatonRules();
            Transaction transaction = _db.Transactions.OrderBy(t => t.DateTime).ToList().Last();
            IEnumerable<Product> transactionProduct = from productTransaction in transaction.ProductTransactions
                                                      select productTransaction.Product;
            var p = from rule in associationRules
                    from pt in transactionProduct
                    where rule.Product.Count() == 1 && rule.Product.First().Id == pt.Id
                    select rule.GoesWith;

            return p;
        }

        private IEnumerable<Product> GetProductListByAssosiatonRules(AssociationRule<int>[] rules)
        {
            IEnumerable<Product> productList = new List<Product>();
            foreach (var rule in rules)
            {
                productList.Concat(BarCodesToProducts(rule.Y));
            }
            return productList;
        }

        private SortedSet<int>[] GetBasicPoductList()
        {
            return GetLastTransactionByBarCode();
        }

        private SortedSet<int>[] GetLastTransactionByBarCode()
        {
            Transaction transactions = _db.Transactions.OrderBy(t => t.DateTime).ToList().Last();
            List<SortedSet<int>> dataset = new List<SortedSet<int>>();
            SortedSet<int> transactionBarCodes = new SortedSet<int>();
            foreach (var producrTransaction in transactions.ProductTransactions)
            {
                transactionBarCodes.Add(producrTransaction.Product.Id);
            }
            dataset.Add(transactionBarCodes);
            return dataset.ToArray();
        }

        public IEnumerable<IAssociationRule> GetAssosiatonRules()
        {
            SortedSet<int>[] dataset = GetAllTransactionByBarCodes();

            // Create a new a-priori learning algorithm with support 2
            Apriori apriori = new Apriori(threshold: 2, confidence: 0);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<int> classifier = apriori.Learn(dataset);

            AssociationRule<int>[] rules = classifier.Rules;

            return GetAssosiatonRules(rules);
        }

        private IEnumerable<IAssociationRule> GetAssosiatonRules(AssociationRule<int>[] rules)
        {
            List<AssosiatonRule> assosiatonRules = new List<AssosiatonRule>();
            foreach (var rule in rules)
            {
                assosiatonRules.Add(new AssosiatonRule(BarCodesToProducts(rule.X), 
                                                                BarCodesToProducts(rule.Y), 
                                                                rule.Confidence));
            }
            return assosiatonRules;
        }

        private IEnumerable<Product> BarCodesToProducts(SortedSet<int> barcodes)
        {
            IEnumerable<Product> allProducts = _db.Products;
            IEnumerable <Product> products = new List<Product>();
            foreach (var barCode in barcodes)
            {
                products = products.Concat(from product in allProducts
                                            where product.Id == barCode
                                            select product);
            }
            return products;
        }

        private SortedSet<int>[] GetAllTransactionByBarCodes()
        {
            IEnumerable<Transaction> transactions = _db.Transactions;
            List<SortedSet<int>> dataset = new List<SortedSet<int>>();
            foreach (var transaction in transactions)
            {
                SortedSet<int> transactionBarCodes = new SortedSet<int>();
                foreach (var producrTransaction in transaction.ProductTransactions)
                {
                    transactionBarCodes.Add(producrTransaction.Product.Id);
                }
                dataset.Add(transactionBarCodes);
            }
            return dataset.ToArray();
        }

    }
}
