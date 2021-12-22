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
    public class ContentByDroneStatusBelongOrPickOrDeliveryParcelConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Drone drone = (Drone)value;

            if (drone.DroneStatus == DroneStatus.Free)
                return "Belong Parcel";
            else if (drone.DroneStatus == DroneStatus.Delivery && !drone.PInTransfer.IsInWay)
                return "Pick parcel";
            else if(drone.DroneStatus == DroneStatus.Delivery && drone.PInTransfer.IsInWay)
                return "Delivery Parcel";
            return ""; 
        }

       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    
    }
}
