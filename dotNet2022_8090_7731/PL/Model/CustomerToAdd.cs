using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;
namespace PO
{
    public class CustomerToAdd : ObservableBase, IDataErrorInfo
    {
        public CustomerToAdd()
        {
        }

        const int ID_LENGTH = 9;

        const int MIN_LATITUDE = -90;
        const int MAX_LATITUDE = 90;

        const int MIN_LONGITUDE = 0;
        const int MAX_LONGITUDE = 90;

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
                    validityMessages[nameof(Id)] = IdCustomerMessage(_id);
                }
                else if (valid)
                {
                    Set(ref _id, id);
                    validityMessages[nameof(Id)] = IdCustomerMessage(_id);
                }
                else
                {
                    validityMessages[nameof(Id)] = IdMessage("invalid input");
                }
                //RaisePropertyChanged("Id");
            }
        }

        private string _name;

        public string Name
        {
            get => _name;

            set
            {
                Set(ref _name, value);
                validityMessages[nameof(Name)] = NameMessage(value);
            }
        }


        private string _prefix;

        public string Prefix
        {
            get => _prefix;

            set
            {
                Set(ref _prefix, value);
                //validityMessages["Phone"] = PhoneMessage(value);
            }
        }

        private string _phone;

        public string Phone
        {
            get => _phone;

            set
            {
                Set(ref _phone, value);
                validityMessages[nameof(Phone)] = PhoneMessage(value,7);
            }
        }

        private double? _longitude;

        public object Longitude
        {
            get => _longitude == null ? null : _longitude;
            set
            {
                bool valid = double.TryParse((string)value, out double longitude);
                if (value is null or "")
                {
                    Set(ref _longitude, null);
                    validityMessages[nameof(Longitude)] = LocationMessage(_longitude);
                }
                else if (valid)
                {
                    Set(ref _longitude, longitude);
                    validityMessages[nameof(Longitude)] = LocationMessage(_longitude, MIN_LONGITUDE, MAX_LONGITUDE);
                }
                else
                {
                    validityMessages[nameof(Longitude)] = LocationMessage("invalid input");
                }
            }
        }

        private double? _latitude;

        public object Latitude
        {
            get => _latitude == null ? null : _latitude;
            set
            {
                bool valid = double.TryParse((string)value, out double latitude);
                if (value is null or "")
                {
                    Set(ref _latitude, null);
                    validityMessages[nameof(Latitude)] = LocationMessage(_latitude);
                }
                else if (valid)
                {
                    Set(ref _latitude, latitude);
                    validityMessages[nameof(Latitude)] = LocationMessage(_latitude, MIN_LATITUDE, MAX_LATITUDE);
                }
                else
                {
                    validityMessages[nameof(Latitude)] = LocationMessage("invalid input");
                }
            }
        }
        // --------------IDataErrorInfo---------------------
        public string Error => validityMessages.Values.All(value => value == string.Empty) ? string.Empty : "Invalid input";

        public string this[string propertyName]
        {
            get
            {
                if (validityMessages.ContainsKey(propertyName))
                    return validityMessages[propertyName];
                return string.Empty;
            }
        }

        private readonly Dictionary<string, string> validityMessages = new()
        {
            [nameof(Id)] = string.Empty,
            [nameof(Name)] = string.Empty,
            [nameof(Phone)] = string.Empty,
            [nameof(Longitude)] = string.Empty,
            [nameof(Latitude)] = string.Empty
        };
    }
}
