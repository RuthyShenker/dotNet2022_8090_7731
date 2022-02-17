using BO;
using PO;
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
    /// <summary>
    /// A class VisibilityByDroneToBelongOrPickOrArrivalParcelConverter that impliments:IValueConverter.
    /// </summary>
    public class VisibilityByDroneToBelongOrPickOrArrivalParcelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EditDrone drone)
            {
                if (drone.Status == DroneStatus.Free || drone.Status == DroneStatus.Delivery)
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
