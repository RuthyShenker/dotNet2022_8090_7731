
using PO;
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
    /// A class ContentByDroneStatusConverter that impliments:IValueConverter.
    /// </summary>
    public class ContentByDroneStatusConverter :IValueConverter
    {

        /// <summary>
        /// A function that converts status to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>returns string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           DroneStatus status = (DroneStatus)value;
           
            if (status == DroneStatus.Free)
            {
               return "Send To Charge:";
            }
            else if(status == DroneStatus.Maintenance)
            {
                return "Release From Charge:";
            }
            return string.Empty;
        }

        /// <summary>
        /// A function that converts back
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
