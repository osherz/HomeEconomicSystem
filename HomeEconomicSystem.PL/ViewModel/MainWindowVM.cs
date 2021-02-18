using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public class MainWindowVM
    {
        public MainMenuVM MainMenuVM { get; private set; }

        public MainWindowVM()
        {
            MainMenuVM = new MainMenuVM();
        }
    }
}
