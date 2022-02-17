using PL.Model;
using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL.Converters
{
    /// <summary>
    /// A class ContentByStatusOfSimulatorConverter that impliments:IValueConverter.
    /// </summary>
    public class ContentByStatusOfSimulatorConverter : IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            //if (Refresh.workers.ContainsKey(value.)
            {

            }
            //SimulatorStatus status = ((Simulator)value).Status;
            return (bool)value ? "Manual" : "Auto";
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var text = ((System.Windows.Controls.TextBox)values[0]).Text;

            var a = int.TryParse(text, out int droneId);

            return a && Refresh.workers.ContainsKey(droneId) && Refresh.workers[droneId].IsBusy ? "Manual" : "Auto";
            
            //return Refresh.workers[int.Parse(((System.Windows.Controls.TextBox)values[0]).Text)].IsBusy ? "Manual" : "Auto";

            //return Refresh.workers[(int )((System.Windows.Controls.TextBox)values[0]).Text)]. ?"Manual":"Auto";

            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
