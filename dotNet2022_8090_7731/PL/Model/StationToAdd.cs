using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PO.ValidityMessages;

namespace PO
{
    class StationToAdd : ObservableBase, IDataErrorInfo
    {
        const int ID_LENGTH = 4;

        private int? _id;

        public object Id
        {
            get => _id == null ? null : _id;
            set
            {
                bool valid = int.TryParse((string)value, out int id);
                if (value is null or "")
                {
                    Set(ref _id, null);
                    validityMessages["Id"] = IntMessage(null);
                }
                else if (valid)
                {
                    Set(ref _id, Convert.ToInt32(value));
                    validityMessages["Id"] = IntMessage(id, ID_LENGTH);
                }
                else
                {
                    validityMessages["Id"] = IntMessage("invalid input");
                }
                //RaisePropertyChanged("Id");
            }
        }

        private string name;

        public string Name
        {
            get => name;

            set
            {
                Set(ref name, value);
                validityMessages["Name"] = StringMessage(value);
            }
        }

        private double? _longitude;
        const int MIN_LONGITUDE = -180;
        const int MAX_LONGITUDE = 180;
        public object Longitude
        {
            get => _longitude == null ? null : _longitude;
            set
            {
                bool valid = double.TryParse((string)value, out double longitude);
                if (value is null or "")
                {
                    Set(ref _longitude, null);
                    validityMessages["Longitude"] = LocationMessage(null);
                }
                else if (valid)
                {
                    Set(ref _longitude, Convert.ToDouble(value));
                    validityMessages["Longitude"] = LocationMessage(longitude, MIN_LONGITUDE, MAX_LONGITUDE);
                }
                else
                {
                    validityMessages["Longitude"] = LocationMessage("invalid input");
                }
            }
        }

        private double? _latitude;
        const int MIN_LATITUDE = -90;
        const int MAX_LATITUDE = 90;
        public object Latitude
        {
            get => _latitude == null ? null : _latitude;
            set
            {
                bool valid = double.TryParse((string)value, out double latitude);
                if (value is null or "")
                {
                    Set(ref _latitude, null);
                    validityMessages["Latitude"] = LocationMessage(null);
                }
                else if (valid)
                {
                    Set(ref _latitude, Convert.ToDouble(value));
                    validityMessages["Latitude"] = LocationMessage(latitude, MIN_LATITUDE, MAX_LATITUDE);
                }
                else
                {
                    validityMessages["Latitude"] = LocationMessage("invalid input");
                }
            }
        }

        private int? _numPositions;
        public object NumPositions
        {
            get => _numPositions == null ? null : _numPositions;
            set
            {
                bool valid = int.TryParse((string)value, out int numPositions);
                if (value is null or "")
                {
                    Set(ref _numPositions, null);
                    validityMessages["NumPositions"] = IntMessage(null);
                }
                else if (valid)
                {
                    Set(ref _numPositions, Convert.ToInt32(value));
                    validityMessages["NumPositions"] = IntMessage(numPositions);
                }
                else
                {
                    validityMessages["NumPositions"] = IntMessage("invalid input");
                }
            }

            //get => numPositions == null ? null : numPositions;
            //set
            //{
            //    numPositions = (value == "" || value == null) ? null : Convert.ToInt32(value);
            //    RaisePropertyChanged("NumPositions");
            //    validityMessages["NumPositions"] = NumPositionsMessage(numPositions);
            //}
        }

        // --------------IDataErrorInfo---------------------
        public string Error
        {
            get
            {
                return validityMessages.Values.All(value => value == string.Empty) ? string.Empty : "Invalid input";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (validityMessages.ContainsKey(columnName))
                    return validityMessages[columnName];
                return string.Empty;
            }
        }

        private Dictionary<string, string> validityMessages = new()
        {
            ["Id"] = " ",
            ["Name"] = " ",
            ["Longitude"] = " ",
            ["Latitude"] = " ",
            ["NumPositions"] = " ",
        };
    }
}

//public string Error { get; }

//public string this[string propertyName]
//{
//    get
//    {
//        return !validityMessages.ContainsKey(propertyName) ? null :
//            string.Join(Environment.NewLine, validityMessages[propertyName]);
//    }
//}