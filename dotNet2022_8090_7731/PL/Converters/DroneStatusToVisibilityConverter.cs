using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using PO;

namespace PL.Converters
{
    /// <summary>
    /// A class DroneStatusToVisibilityConverter that impliments:IValueConverter.
    /// </summary>
    public class DroneStatusToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// A function that converts Drone status to visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>returns type of visibility</returns>
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {
            DroneStatus status = (DroneStatus)value;
            if (status==DroneStatus.Free|| status == DroneStatus.Maintenance)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// A function that converts back
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value,Type targetType,object parameter,CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

