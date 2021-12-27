using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    class DroneToAdd : INotifyPropertyChanged, IDataErrorInfo
    {
        private int? _id;
        public int Id
        {
            get => _id == null ? 0 : (int)_id;
            set
            { 
                _id = value;
                RaisePropertyChanged("Id");
                errors["Id"] = IsIdValid(value);
              
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged("Model");
            }
        }

        private BO.WeightCategories _maxWeight;
        public BO.WeightCategories MaxWeight
        {
            get { return _maxWeight; }
            set
            {
                _maxWeight = value;
                RaisePropertyChanged("Weight");
            }
        }

        // Validates the Id property, updating the errors collection as needed.
        public string IsIdValid(int value)
        {
            if (value ==null)
            {
                return "Value is reqired";
            }
            if (value< Math.Pow(10, 3))
            {
                return "1 Id must contain 4 digits";
            }
            if (value> Math.Pow(10, 4))
            {
                return "2 Id must contain 4 digits";
            }
            else
            {
                return "";
            }
        
        }

        private int _stationId;
        public int StationId
        {
            get { return _stationId; }
            set
            {
                _stationId = value;
                RaisePropertyChanged("Station");
            }
        }

        public string Error => "1";

        private Dictionary<string, string> errors = new Dictionary<string, string>()
        {
            ["Id"] = "",
            ["Model"] = "",
            ["MaxWeight"] = "",
            ["StationId"] = "",
        };

        public string this[string propertyName]
        {
            get
            {
                return !errors.ContainsKey(propertyName) ? null :
                    string.Join(Environment.NewLine, errors[propertyName]);
            }
        }

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


        //if (value < 5)
        //{
        //    errors["ID"] = "Value cannot be less than 5";
        //}

        ////else RemoveError("Id", ID_ERROR);

        //if (value > 10) AddError("Id", IdGreater, true);
        //else RemoveError("Id", ID_WARNING);


        // Adds the specified error to the errors collection if it is not already 
        // present, inserting it in the first position if isWarning is false. 
        //public void AddError(string propertyName, string error, bool isWarning)
        ////AddError("Id", ID, false);

        //{
        //    if (!errors.ContainsKey(propertyName))
        //        errors[propertyName] = new List<string>();

        //    if (!errors[propertyName].Contains(error))
        //    {
        //        if (isWarning) errors[propertyName].Add(error);
        //        //else errors[propertyName].Insert(0, error);
        //    }
        //}

        //// Removes the specified error from the errors collection if it is present. 
        //public void RemoveError(string propertyName, string error)
        //{
        //    if (errors.ContainsKey(propertyName) &&
        //        errors[propertyName].Contains(error))
        //    {
        //        errors[propertyName].Remove(error);
        //        if (errors[propertyName].Count == 0) errors.Remove(propertyName);
        //    }
        //}
        //private Dictionary<String, List<String>> errors =
        //    new Dictionary<string, List<string>>();
        //private const string ID = "Value cannot be less than 5.";
        //private const string IdGreater = "Value should not be greater than 10.";
        //private const string MaxWeightt = "Value must not contain any spaces.";
        //private const string StationIdd= "Value should be 5 characters or less.";
        //public int Id
        //{
        //    get { return _id; }
        //    set
        //    {
        //        _id = value;
        //        RaisePropertyChanged("Id");
        //    }
        //}