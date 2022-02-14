using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class DroneToUpdate : INotify
    {
        //    int id;
        string model;
        //BO.WeightCategories weight;
        //double batteryStatus;
        //BO.DroneStatus status;
        //BO.Location location;
        //BO.ParcelInTransfer parcelInTransfer;
        public int Id { get; init; }
        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                RaisePropertyChanged(nameof(Model));
            }
        }
        public BO.WeightCategories Weight { get; set; }

        public double BatteryStatus { get; set; }
        public BO.DroneStatus DStatus { get; set; }
        public BO.Location Location { get; set; }
        public BO.ParcelInTransfer ParcelInTransfer { get; set; }

        
        //protected void RaisePropertyChanged(string propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }

        //}
    }
}
