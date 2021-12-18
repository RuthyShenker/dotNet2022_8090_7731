using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL
{
    public class ConvertVisibilityByDroneToBelongOrPickOrArrivalParcel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Drone drone = (BO.Drone)value;

            if (drone.DroneStatus == BO.DroneStatus.Free|| drone.DroneStatus == BO.DroneStatus.Delivery)
                return  Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
