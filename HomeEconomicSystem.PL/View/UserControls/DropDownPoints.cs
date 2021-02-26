using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.View.UserControls
{
    public class DropDownPoint
    {
        static DropDownPoint()
        {
            //In this case we are plotting our own point to have 
            //more control over the current plot
            //configuring a custom type is quite simple

            //first we define a mapper
            var mapper = Mappers.Pie<DropDownPoint>()
                .Value(x => x.Value);//use the value property in the plot

            //then we save the mapper globally, there are many ways
            //so configure a series, for more info see:
            //https://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration
            Charting.For<DropDownPoint>(mapper);
        }

        /// <summary>
        /// Gets or sets the value to plot
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value
        {
            get { return Content.Sum(item=>item.Value); }
        }

        /// <summary>
        /// Gets or sets the content, all the values that represent this point
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public IReadOnlyDictionary<string, double> Content { get; set; }

    }
}
