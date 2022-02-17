using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BO;

namespace PL.Converters
{
    /// <summary>
    /// A class DroneStatusToVisibilityConverter that impliments:IValueConverter.
    /// </summary>
    public class DroneStatusToVisibilityConverter : IValueConverter
    {
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
        public object ConvertBack(object value,Type targetType,object parameter,CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

