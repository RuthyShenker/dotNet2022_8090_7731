using BO;
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
    public class ContentByDroneStatusBelongOrPickOrDeliveryParcelConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EditDrone drone)
            {
                if (drone.Status == DroneStatus.Free)
                    return "Belong Parcel";
                else if (drone.Status == DroneStatus.Delivery && (drone.ParcelInTransfer == null || !drone.ParcelInTransfer.IsInWay)) 
                    return "Pick parcel";
                else if (drone.Status == DroneStatus.Delivery && drone.ParcelInTransfer.IsInWay)
                    return "Delivery Parcel";
            }
            return ""; 
        }

       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    
    }
}
