using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL
{
    public class ConvertContentByDroneStatus :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.DroneStatus status = (BO.DroneStatus)value;
            object obj;
            if (status == BO.DroneStatus.Free)
            {
                 obj = "Send To Charge:";
                return obj;
            }
            else//status == BO.DroneStatus.Maintenance
            {
                 obj = "Release From Charge:";
                return obj;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
