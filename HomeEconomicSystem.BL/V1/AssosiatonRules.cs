using HomeEconomicSystem.BE;
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
        public void CreateShopingListRecommendation(string path)
        {
            File.Create("a.pdf").Close();
        }

        public IEnumerable<IAssosiatonRule> GetAssosiatonRules()
        {
            var Products = new[]
            {
                new Product
                {
                    Id = 1,
                    Name = "קטשופ"
                },
                new Product
                {
                    Id = 2,
                    Name = "חלב"
                }
            };

            for (int i = 0; i < 10; i++)
            {
                yield return new AssosiatonRule
                {
                    Product = new[] { Products[0] },
                    GoesWith = new[] { Products[1] },
                    Probability = i / 10.0f
                };
            }
        }
    }
}
