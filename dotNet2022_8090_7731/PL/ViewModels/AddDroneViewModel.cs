using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{
    class AddDroneViewModel
    {
        public DroneToAdd Drone { get; set; } = new();

        public Array WeightOptions { get; set; } = Enum.GetValues(typeof(BO.WeightCategories));
        public List<int> StationOptions { get; set; }
        public RelayCommand<object> AddDroneCommand { get; set; }

        public AddDroneViewModel()
        {
            StationOptions = BlApi.BlFactory.GetBl().AvailableSlots().Select(station => station.Id).ToList();
            AddDroneCommand = new RelayCommand<object>(AddDrone, param => Drone.Error != "");
        }

        private void AddDrone(object parameters)
        {
            MessageBox.Show("Add Drone");
        }
    }
}
