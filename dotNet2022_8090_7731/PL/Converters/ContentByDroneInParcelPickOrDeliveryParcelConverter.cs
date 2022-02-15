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
    public class ContentByDroneInParcelPickOrDeliveryParcelConverter : IValueConverter
    {



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is EditParcel parcel)
            {
                if (parcel?.PickingUp == null)
                    return "Pick Parcel";
                else if (parcel?.Arrival == null)
                    return "Delivery Parcel";
                else return "";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


            
                

