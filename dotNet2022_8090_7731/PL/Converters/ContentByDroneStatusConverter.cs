using BO;
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           DroneStatus status = (DroneStatus)value;
            object obj="";
            if (status == DroneStatus.Free)
            {
                 obj = "Send To Charge:";
            }
            else if(status == DroneStatus.Maintenance)
            {
                 obj = "Release From Charge:";
            }
            return obj;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
