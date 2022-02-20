

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
    /// A public class EditDrone impliments:ObservableBase, IDataErrorInfo
    /// includes:
    ///Id
    ///Model
    ///Weight
    ///BatteryStatus
    ///Status
    ///Location
    ///ParcelInTransfer
    ///Automatic
    ///Distance
    /// </summary>
    /// 
    public class EditDrone : ObservableBase, IDataErrorInfo
    {
        public int Id { get; init; }
        
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

        private WeightCategories weight;
        public WeightCategories Weight
        {
            get => weight;
            set => Set(ref weight, value);
        }

        private double batteryStatus;
        public double BatteryStatus
        {
            get => batteryStatus;
            set => Set(ref batteryStatus, value);
        }

        private DroneStatus status;
        public DroneStatus Status
        {
            get => status;
            set => Set(ref status, value);
        }

       
        private Location location;

        public Location Location
        {
            get => location;
            set => Set(ref location, value);
        }

        private ParcelInTransfer parcelInTransfer;

        public ParcelInTransfer ParcelInTransfer
        {
            get { return parcelInTransfer; }
            set { Set(ref parcelInTransfer, value);  }
        }

        private bool simulatorStatus;

        public bool SimulatorStatus
        {
            get { return simulatorStatus; }
            set { Set(ref simulatorStatus, value); }
        }


        private bool automatic;
        public bool Automatic
        {
            get => automatic;
            set => Set(ref automatic, value);
        }

        private object info;
        public object Info
        {
            get => info;
            set => Set(ref info, value);
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
            [nameof(Model)] = string.Empty,
        };
    }
}
