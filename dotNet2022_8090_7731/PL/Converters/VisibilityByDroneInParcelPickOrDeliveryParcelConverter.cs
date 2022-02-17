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
    /// A class VisibilityByDroneInParcelPickOrDeliveryParcelConverter that impliments:IValueConverter.
    /// </summary>
    class VisibilityByDroneInParcelPickOrDeliveryParcelConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is EditParcel parcel)
            {
                if (parcel.BelongParcel.HasValue &&(parcel?.PickingUp == null || parcel?.Arrival == null))
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
