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
    public class StationToAdd : ObservableBase, IDataErrorInfo
    {
        const int ID_LENGTH = 4;
        const int MIN_LONGITUDE = 0;
        const int MAX_LONGITUDE = 90;
        const int MIN_LATITUDE = -90;
        const int MAX_LATITUDE = 90;

        private int? _id;
        public object Id
        {
            get => _id == null ? null : _id;
            set
            {
                bool valid = int.TryParse((string)value, out _);
                if (value is null or "")
                {
                    Set(ref _id, null);
                    validityMessages["Id"] = IdMessage(_id);
                }
                else if (valid)
                {
                    Set(ref _id, Convert.ToInt32(value));
                    validityMessages["Id"] = IdMessage(_id, ID_LENGTH);
                }
                else
                {
                    validityMessages["Id"] = IdMessage("invalid input");
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
                validityMessages[nameof(Name)] = value is null or "" ? RequiredMessage() :
                                                                StringMessage(value);
            }
        }

        private object _longitude;
        public object Longitude
        {
            get => _longitude == null ? null : _longitude;
            set
            {
                bool valid = double.TryParse((string)value, out _);
                if (value is null or "")
                {
                    Set(ref _longitude, null);
                    validityMessages[nameof(Longitude)] = LocationMessage(_longitude);
                }
                else if (valid)
                {
                    Set(ref _longitude, Convert.ToDouble(value));
                    validityMessages[nameof(Longitude)] = LocationMessage(_longitude, MIN_LONGITUDE, MAX_LONGITUDE);
                }
                else
                {
                    validityMessages[nameof(Longitude)] = LocationMessage("invalid input");
                }
                _longitude = value;
            }
        }

        private object _latitude;
        public object Latitude
        {
            get => _latitude == null ? null : _latitude;
            set
            {
                bool valid = double.TryParse((string)value, out _);
                if (value is null or "")
                {
                    Set(ref _latitude, null);
                    validityMessages[nameof(Latitude)] = LocationMessage(_latitude);
                }
                else if (valid)
                {
                    Set(ref _latitude, Convert.ToDouble(value));
                    validityMessages[nameof(Latitude)] = LocationMessage(_latitude, MIN_LATITUDE, MAX_LATITUDE);
                }
                else
                {
                    validityMessages[nameof(Latitude)] = LocationMessage("invalid input");
                }
                _latitude = value;
            }
        }

        private int? _numPositions;
        public object NumPositions
        {
            get => _numPositions == null ? null : _numPositions;
            set
            {
                bool valid = int.TryParse((string)value, out _);
                if (value is null or "")
                {
                    Set(ref _numPositions, null);
                    validityMessages[nameof(NumPositions)] = numPositionsMessage(_numPositions);
                }
                else if (valid)
                {
                    Set(ref _numPositions, Convert.ToInt32(value));
                    validityMessages[nameof(NumPositions)] = numPositionsMessage(_numPositions);
                }
                else
                {
                    validityMessages[nameof(NumPositions)] = numPositionsMessage("invalid input");
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
        public string Error => validityMessages.Values.All(value => value == string.Empty) ? string.Empty : "Invalid input";

        public string this[string columnName]
        {
            get
            {
                if (validityMessages.ContainsKey(columnName))
                    return validityMessages[columnName];
                return string.Empty;
            }
        }
        /// <summary>
        /// A dictionary of validation:
        /// </summary>
        private readonly Dictionary<string, string> validityMessages = new()
        {
            [nameof(Id)] = string.Empty,
            [nameof(name)] = string.Empty,
            [nameof(Longitude)] = string.Empty,
            [nameof(Latitude)] = string.Empty,
            [nameof(NumPositions)] = string.Empty,
        };
    }
}

