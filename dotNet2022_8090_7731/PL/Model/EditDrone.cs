using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class EditDrone
    {
        public EditDrone(int id, string model, WeightCategories weight, double batteryStatus, 
            DroneStatus status, PO.Location location, ParcelInTransfer parcelInTransfer)
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
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double BatteryStatus { get; set; }
        public DroneStatus Status { get; set; }
        public Location Location { get; set; }
        public ParcelInTransfer ParcelInTransfer { get; set; }
    }
}
