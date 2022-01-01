using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;

namespace PO
{
    public class DroneToAdd : ObservableBase, IDataErrorInfo
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

        private string _model;

        public string Model
        {
            get => _model;

            set
            {
                Set(ref _model, value);
                validityMessages["Model"] = value is null or "" ? RequiredMessage() :
                                                                StringMessage(value);
            }
        }

        private BO.WeightCategories _maxWeight;
        public BO.WeightCategories MaxWeight
        {
            get => _maxWeight;
            set => Set(ref _maxWeight, value);
        }

        private int _stationId;
        public int StationId
        {
            get { return _stationId; }
            set
            {
                Set(ref _stationId, value);
                validityMessages["StationId"] = "";
            }
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

        private Dictionary<string, string> validityMessages = new Dictionary<string, string>()
        {
            ["Id"] = " ",
            ["Model"] = " ",
            ["MaxWeight"] = "",
            ["StationId"] = " ",
        };

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
        //        if (isWarning) errors[propertyName].AddCustomer(error);
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