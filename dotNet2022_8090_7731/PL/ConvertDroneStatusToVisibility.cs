using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace PL
{
    public class ConvertVisibilityByDroneStatus : IValueConverter
    {
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {
            BO.DroneStatus status = (BO.DroneStatus)value;
            if (status==BO.DroneStatus.Free|| status == BO.DroneStatus.Maintenance)
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

