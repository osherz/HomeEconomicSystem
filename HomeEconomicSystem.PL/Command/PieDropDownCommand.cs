using HomeEconomicSystem.PL.View.UserControls;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.Command
{
    public class PieDropDownCommand : ICommand
    { 
        private Action<DropDownPoint> _action;

        public PieDropDownCommand(Action<DropDownPoint> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            var chartPoint = (ChartPoint)parameter;
            var dropDownPoint = (DropDownPoint)chartPoint.Instance;

            _action(dropDownPoint);
        }

        public bool CanExecute(object parameter)
        {
            var chartPoint = (ChartPoint)parameter;
            return chartPoint.Instance is DropDownPoint;
        }

        public event EventHandler CanExecuteChanged;
    }
}
