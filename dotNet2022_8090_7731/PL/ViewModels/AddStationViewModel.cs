using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PO;
namespace PL
{
    class AddStationViewModel
    {
        public StationToAdd station { get; set; } = new();
        public RelayCommand<object> AddStationCommand { get; set; }
        public AddStationViewModel()
        {
            AddStationCommand = new RelayCommand<object>(AddStation, param => station.Error == null);
        }

        private void AddStation(object obj)
        {
            MessageBox.Show("AddCustomer station");
            //BlApi.BlFactory.GetBl().AddingBaseStation(station)
        }
    }
}
