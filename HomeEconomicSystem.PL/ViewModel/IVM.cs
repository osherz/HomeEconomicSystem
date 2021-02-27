using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public interface IVM
    {
        /// <summary>
        /// State of an innet state-machine changed.
        /// </summary>
        event EventHandler<string> InnerStateChanged;
    }
}
