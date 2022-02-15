using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PO.ValidityMessages;

namespace PO
{
    public class EditDrone : ObservableBase, IDataErrorInfo
    {
        public EditDrone(int id, string model, WeightCategories weight, double batteryStatus,
            DroneStatus status, Location location, ParcelInTransfer parcelInTransfer)
        {
            Id = id;
            Model = model;
            Weight = weight;
            BatteryStatus = batteryStatus;
            Status = status;
            Location = location;
            ParcelInTransfer = parcelInTransfer;
        }

        public int Id { get; set; }
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
        public WeightCategories Weight { get; set; }
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

        //public DroneStatus Status { get; set; }
        public Location Location { get; set; }
        public ParcelInTransfer ParcelInTransfer { get; set; }

        private bool automatic;
        public bool Automatic
        {
            get => automatic;
            set => Set(ref automatic, value);
        }

        private double distance;

        public double Distance
        {
            get => distance;
            set => Set(ref distance, value);
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
        private Dictionary<string, string> validityMessages = new Dictionary<string, string>()
        {
            [nameof(Model)] = string.Empty,
        };
    }
}
