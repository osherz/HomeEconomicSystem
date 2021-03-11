using Accord.MachineLearning.Rules;
using HomeEconomicSystem.BE;
using HomeEconomicSystem.Dal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            File.Create("a.pdf").Close();
        }

        public IEnumerable<IAssociationRule> GetAssosiatonRules()
        {
            SortedSet<int>[] dataset = GetAllTransactionByBarCodes();

            // Create a new a-priori learning algorithm with support 3
            Apriori apriori = new Apriori(threshold: 3, confidence: 0);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<int> classifier = apriori.Learn(dataset);

            AssociationRule<int>[] rules = classifier.Rules;

            return GetAssosiatonRules(rules);
        }

        private IEnumerable<IAssociationRule> GetAssosiatonRules(AssociationRule<int>[] rules)
        {
            IEnumerable<AssosiatonRule> assosiatonRules = new List<AssosiatonRule>();
            foreach (var rule in rules)
            {
                assosiatonRules.ToList().Add(new AssosiatonRule(BarCodesToProducts(rule.X), 
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
                products.Concat(from product in allProducts
                                where product.Id == barCode
                                select product);
            }
            return products;
        }

        private SortedSet<int>[] GetAllTransactionByBarCodes()
        {
            IEnumerable<Transaction> transactions = _db.Transactions;
            SortedSet<int>[] dataset = new SortedSet<int>[transactions.ToList().Count];
            foreach (var transaction in transactions)
            {
                SortedSet<int> transactionBarCodes = new SortedSet<int>();
                foreach (var producrTransaction in transaction.ProductTransactions)
                {
                    transactionBarCodes.Add(producrTransaction.Product.Id);
                }
                dataset = dataset.Append(transactionBarCodes).ToArray();
            }
            return dataset;
        }

    }
}
