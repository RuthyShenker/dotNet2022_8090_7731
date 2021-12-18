using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL
{
    public class ConvertContentByDroneStatusBelongOrPickOrDeliveryParcel : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Drone drone = (BO.Drone)value;
            if (drone == null)
            {
                return "";
            }
            
            if (drone.DroneStatus == BO.DroneStatus.Free)
                return "Belong Parcel";
            else if (drone.DroneStatus == BO.DroneStatus.Delivery && !drone.PInTransfer.IsInWay)
                return "Pick parcel";
            else if(drone.DroneStatus == BO.DroneStatus.Delivery && drone.PInTransfer.IsInWay)
                return "Delivery Parcel";
            return ""; 
        }

       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    
    }
}
