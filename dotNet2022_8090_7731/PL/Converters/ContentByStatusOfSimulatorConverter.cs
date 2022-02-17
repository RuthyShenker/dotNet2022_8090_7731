
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
    /// A class ContentByStatusOfSimulatorConverter that impliments:IMultiValueConverter.
    /// </summary>
    public class ContentByStatusOfSimulatorConverter : IValueConverter
    {
        /// <summary>
        /// A function that converts string by status of simulator
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>returns string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var droneId = ((PO.EditDrone)value).Id;

            return Refresh.workers.ContainsKey(droneId) && Refresh.workers[droneId].CancellationPending ? "Manual" : "Auto";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// A function that converts back
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="targetType"></param>
        ///// <param name="parameter"></param>
        ///// <param name="culture"></param>
        ///// <returns></returns>
        //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        //{
        //    var text = ((System.Windows.Controls.TextBox)values[0]).Text;

        //    var result = int.TryParse(text, out int droneId);

        //    return result && Refresh.workers.ContainsKey(droneId) && Refresh.workers[droneId].IsBusy ? "Manual" : "Auto";
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }

}
