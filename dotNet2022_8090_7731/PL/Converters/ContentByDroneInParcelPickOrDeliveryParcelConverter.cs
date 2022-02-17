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
    /// A class ContentByDroneInParcelPickOrDeliveryParcelConverter that impliments:IValueConverter.
    /// </summary>
    public class ContentByDroneInParcelPickOrDeliveryParcelConverter : IValueConverter
    {
        /// <summary>
        /// A function that gets parcel of type EditParcel and converts the data of PickingUp/Arrival to appropriate string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>appropriate string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EditParcel parcel)
            {
                if (parcel?.PickingUp == null)
                    return "Pick Parcel";
                else if (parcel?.Arrival == null)
                    return "Delivery Parcel";
            }
            //TODO:
            return "";
        }

        /// <summary>
        /// A function that converts back.
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


            
                

