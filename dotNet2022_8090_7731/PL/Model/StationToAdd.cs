using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PO
{
    class StationToAdd : INotifyPropertyChanged, IDataErrorInfo
    {
        const int ID_LENGTH = 4;

        private int? id;

        public object Id
        {
            get => id == null ? null : id;
            set
            {
                id = (value == "" || value == null) ? null : Convert.ToInt32(value);
                RaisePropertyChanged("Id");
                validityMessages["Id"] = IdMessage(id, ID_LENGTH);
            }
        }

        private string name;

        public string Name
        {
            get => name;
            
            set
            {
                name = value;
                RaisePropertyChanged("Name");
                validityMessages["Name"] = NameMessage(value);
            }
        }

        private double? longitude;
        const int MIN_LONGITUDE = -180;
        const int MAX_LONGITUDE = 180;
        public object Longitude
        {
            get => longitude == null ? null : longitude;
            set
            {

                //!((string)value).All(c =>( char.IsDigit(c)||c.Equals('.')) 

                //if (value.GetType().IsValueType)
                //{
                //    var a = 5;
                //}
                longitude = (value is "" or null) ? null : (double)Convert.ToDecimal(value);
                RaisePropertyChanged("Longitude");
                validityMessages["Longitude"] = LocationMessage(longitude, MIN_LONGITUDE, MAX_LONGITUDE);
            }
        }

        private double? latitude;
        const int MIN_LATITUDE = -90;
        const int MAX_LATITUDE = 90;
        public object Latitude
        {
            get => latitude == null ? null : latitude;
            set
            {
                var a = value.GetType();
                latitude = (value == "" || value == null) ? null : Convert.ToDouble(value);
                RaisePropertyChanged("Latitude");
                validityMessages["Latitude"] = LocationMessage(latitude, MIN_LATITUDE, MAX_LATITUDE);
            }
        }


        private int? numPositions;
        public object NumPositions
        {
            get => numPositions == null ? null : numPositions;
            set
            {
                numPositions = (value == "" || value == null) ? null : Convert.ToInt32(value);
                RaisePropertyChanged("NumPositions");
                validityMessages["NumPositions"] = NumPositionsMessage(numPositions);
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

        private string NumPositionsMessage(object value)
        {
            return value switch
            {
                null => "Feild is required",
                _ => "",
            };
        }

        private string LocationMessage(object value, int min, int max)
        {
            return value == null ? "Feild is required" :
                (double)value > max ? $"Max value is {max}" :
                (double)value < min ? $"Min value is {min}" :
                "";
        }

        private string NameMessage(string value)
        {
            return (value == "" || value == null) ? "Feild is required" :
                   !value.All(c => char.IsLetter(c)) ? "Name has to contain only letters" :
                   "";
        }

        private string IdMessage(object value, int length)
        {
            int maxLength = (int)Math.Pow(10, length);
            return value switch
            {
                null => "Feild is required",
                > 10000 or < 1000 => "Input must contain 4 digits",
                _ => "",
            };
        }

        private Dictionary<string, string> validityMessages = new()
        {
            ["Id"] = " ",
            ["Name"] = " ",
            ["Longitude"] = " ",
            ["Latitude"] = " ",
            ["NumPositions"] = " ",
        };

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
