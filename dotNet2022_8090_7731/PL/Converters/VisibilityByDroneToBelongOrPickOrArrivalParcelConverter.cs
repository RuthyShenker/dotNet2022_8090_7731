using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL.Converters
{
    public class VisibilityByDroneToBelongOrPickOrArrivalParcelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Drone drone = (Drone)value;

            if (drone.DroneStatus == DroneStatus.Free|| drone.DroneStatus == DroneStatus.Delivery)
                return  Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
