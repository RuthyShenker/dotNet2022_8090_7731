

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
        /// <summary>
        /// A constructor of EditDrone with params.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="weight"></param>
        /// <param name="batteryStatus"></param>
        /// <param name="status"></param>
        /// <param name="location"></param>
        /// <param name="parcelInTransfer"></param>

        public int Id { get; init; }
        //private int id;

        //public int Id
        //{
        //    get { return id; }
        //    set { Set(ref id, value); }
        //}

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

        //public DroneStatus Status { get; set; }
        private Location location;

        public Location Location
        {
            get => location;
            set => Set(ref location, value);
        }

        //public override string ToString()
        //{
        //    double latitude = Latitude;
        //    double longitude = Longitude;
        //    string lat()
        //    {
        //        string ch = "N";
        //        if (latitude < 0)
        //        {
        //            ch = "S";
        //            latitude = -latitude;
        //        }
        //        int deg = (int)latitude;
        //        int min = (int)(60 * (latitude - deg));
        //        double sec = (latitude - deg) * 3600 - min * 60;
        //        return $"{deg}° {min}′ {sec}″ {ch}";
        //    }

        //    string lng()
        //    {
        //        string ch = "E";
        //        if (longitude < 0)
        //        {
        //            ch = "W";
        //            longitude = -longitude;
        //        }

        //        int deg = (int)longitude;
        //        int min = (int)(60 * (longitude - deg));
        //        double sec = (longitude - deg) * 3600 - min * 60;
        //        return $"{deg}° {min}′ {sec}″ {ch}";
        //    }

        //    return $"{lat()} {lng()}";
        //}

        //public Location Location { get; set; }
        //public ParcelInTransfer ParcelInTransfer { get; set; }

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

        /// <summary>
        /// A dictionary of validation.
        /// </summary>
        private readonly Dictionary<string, string> validityMessages = new()
        {
            [nameof(Model)] = string.Empty,
        };
    }
}
