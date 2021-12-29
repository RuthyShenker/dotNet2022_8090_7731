//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static PO.ValidityMessages;

//using static PO.ObservableBase;
//namespace PO
//{
//    class Class1 : ObservableBase
//    {
//        private double? _longitude;
//        const int MIN_LONGITUDE = -180;
//        const int MAX_LONGITUDE = 180;
//        public object Longitude
//        {
//            get => _longitude == null ? null : _longitude;
//            set
//            {
//                bool valid = double.TryParse((string)value, out double longitude);
//                if (value is null or "")
//                {
//                    Set(ref _longitude, null);
//                  PO.DroneToAdd. DroneToAdd.validityMessages["Longitude"] = LocationMessage(null);
//                }
//                else if (valid)
//                {
//                    Set(ref _longitude, Convert.ToDouble(value));
//                    validityMessages["Longitude"] = LocationMessage(longitude, MIN_LONGITUDE, MAX_LONGITUDE);
//                }
//                else
//                {
//                    validityMessages["Longitude"] = LocationMessage("invalid input");
//                }
//            }
//        }
//    }
//}
