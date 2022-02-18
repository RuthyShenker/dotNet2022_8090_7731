
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;

namespace PO
{
    /// <summary>
    /// A public class DroneToAdd impliments: ObservableBase, IDataErrorInfo
    /// includes:
    /// Id
    /// Model
    /// MaxWeight
    /// StationId

    /// </summary>
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
                    validityMessages[nameof(Id)] = IdMessage(_id);
                }
                else if (valid)
                {
                    Set(ref _id, Convert.ToInt32(value));
                    validityMessages[nameof(Id)] = IdMessage(_id, ID_LENGTH);
                }
                else
                {
                    validityMessages[nameof(Id)] = IdMessage("invalid input");
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
                validityMessages[nameof(Model)] = value is null or "" ? RequiredMessage() :
                                                                OnlyStringAndNumberMessage(value);
            }
        }

        private WeightCategories _maxWeight;
        public WeightCategories MaxWeight
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
                validityMessages[nameof(StationId)] = string.Empty;
            }
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
        /// A dictionary of validation.
        /// </summary>
        private readonly Dictionary<string, string> validityMessages = new()
        {
            [nameof(Id)] = string.Empty,
            [nameof(Model)] = string.Empty,
            [nameof(MaxWeight)] = string.Empty,
            [nameof(StationId)] = string.Empty,
        };

    }
}

