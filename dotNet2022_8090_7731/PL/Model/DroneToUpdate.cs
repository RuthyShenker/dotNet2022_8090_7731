using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class DroneToUpdate : INotifyPropertyChanged
    {
        int id;
        string model;
        BO.WeightCategories weight;
        double batteryStatus;
        BO.DroneStatus status;
        BO.Location location;
        BO.ParcelInTransfer parcelInTransfer;
        public int Id { get => id; }
        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                RaisePropertyChanged("Model");
            }
        }
        public BO.WeightCategories Weight { get => weight; }

        public double BatteryStatus { get => batteryStatus; }
        public BO.DroneStatus DStatus { get => status; }
        public BO.Location Location { get => location; }
        public BO.ParcelInTransfer ParcelInTransfer { get => parcelInTransfer; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
