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
    /// <summary>
    /// A class ContentByDroneStatusBelongOrPickOrDeliveryParcelConverter that impliments:IValueConverter.
    /// </summary>
    public class ContentByDroneStatusBelongOrPickOrDeliveryParcelConverter : IValueConverter
    {
        /// <summary>
        /// A function that gets drone of type EditDrone and converts status to string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>returns string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EditDrone drone)
            {
                if (drone.Status ==DroneStatus.Free)
                    return "Belong Parcel";
                else if (drone.Status == DroneStatus.Delivery && (drone.ParcelInTransfer == null || !drone.ParcelInTransfer.IsInWay)) 
                    return "Pick parcel";
                else if (drone.Status == DroneStatus.Delivery && drone.ParcelInTransfer.IsInWay)
                    return "Delivery Parcel";
            }
            return ""; 
        }

       /// <summary>
       /// A function that converts back
       /// </summary>
       /// <param name="value"></param>
       /// <param name="targetType"></param>
       /// <param name="parameter"></param>
       /// <param name="culture"></param>
       /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    
    }
}
