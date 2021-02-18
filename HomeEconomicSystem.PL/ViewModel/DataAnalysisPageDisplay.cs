using HomeEconomicSystem.PL.Extensions;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    class DataAnalysisPageDisplay : IPageDisplay
    {
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public DataAnalysisPageDisplay(StateMachine stateMachine)
        {
            Content = new View.DataAnalysisView();
            MenuItems = new List<MenuItem>
            {
                new MenuItem("שמורים", PackIconKind.StarCircle, stateMachine.CreateCommand(Triggers.DataAnalysisSelected)),
                new MenuItem("AR", PackIconKind.ChartSankeyVariant, stateMachine.CreateCommand(Triggers.DataAnalysisSelected)),
                 new MenuItem("טיוטה", PackIconKind.File, stateMachine.CreateCommand(Triggers.DataAnalysisSelected))
            };
        }
    }
}
