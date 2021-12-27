using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    class StationToAdd : INotifyPropertyChanged, IDataErrorInfo
    {
        const int ID_LENGTH = 8;
        
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
                validityMessages["Id"] = IdMessage(value, ID_LENGTH);
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

        private string longitude;
        const int MIN_LONGITUDE = -180;
        const int MAX_LONGITUDE = 180;
        public string Longitude
        {
            get => longitude;
            set
            {
                name = value;
                RaisePropertyChanged("Longitude");
                validityMessages["Longitude"] = LocationMessage(value, MIN_LONGITUDE, MAX_LONGITUDE);
            }
        }

        private string latitude;
        const int MIN_LATITUDE = -90;
        const int MAX_LATITUDE = 90;
        public string Latitude
        {
            get => latitude;
            set
            {
                latitude = value;
                RaisePropertyChanged("Latitude");
                validityMessages["Latitude"] = LocationMessage(value, MIN_LATITUDE, MAX_LATITUDE);
            }
        }

        private int numPositions;

        public int NumPositions
        {
            get => numPositions;
            set
            {
                numPositions = value;
                RaisePropertyChanged("NumPositions");
                validityMessages["NumPositions"] = NumPositionsMessage(value);
            }
        }

        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                return !validityMessages.ContainsKey(propertyName) ? null :
                    string.Join(Environment.NewLine, validityMessages[propertyName]);
            }
        }

        private string NumPositionsMessage(int value)
        {
            throw new NotImplementedException();
        }

        private string LocationMessage(string value, int min, int max)
        {
            throw new NotImplementedException();
        }

        private string NameMessage(string value)
        {
            throw new NotImplementedException();
        }

        private string IdMessage(int value, int length)
        {
            int maxLength  = (int)Math.Pow(10, length);
            switch (value)
            {
                default:
                    return "";
            }
        }

        private Dictionary<string, string> validityMessages = new()
        {
            ["Id"] = "",
            ["NameStation"] = "",
            ["Longitude"] = "",
            ["Latitude"] = "",
            ["NumPositions"] = "",
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
