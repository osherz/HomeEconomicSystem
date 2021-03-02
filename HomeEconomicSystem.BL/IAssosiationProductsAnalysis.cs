using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HomeEconomicSystem.BL
{
    public interface IAssosiationProductsAnalysis
    {
        /// <summary>
        /// creatte PDF file that contains the recomended shoping list.
        /// </summary>
        /// <param name="path">the path to the location of the file</param>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        void CreateShopingListRecommendation(string path);

        /// <summary>
        /// returns all the assosiation rules of all products.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException">Raise an exception if action failed.</exception>
        IEnumerable<IAssociationRule> GetAssosiatonRules();
    }
}
