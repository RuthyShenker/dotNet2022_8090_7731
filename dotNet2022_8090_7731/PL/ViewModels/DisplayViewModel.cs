using PL.View;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    public class DisplayViewModel
    {
        BlApi.IBL bl;
        public RelayCommand<object> CustomerListViewCommand;
        public RelayCommand<object> DroneListViewCommand;
        public RelayCommand<object> StationListViewCommand;
        public RelayCommand<object> ParcelListViewCommand;
        public DisplayViewModel()
        {
            bl = BlApi.BlFactory.GetBl();
            CustomerListViewCommand = new RelayCommand<object>(ShowCustomerListView);
            DroneListViewCommand = new RelayCommand<object>(ShowDroneListView);
            StationListViewCommand = new RelayCommand<object>(ShowStationListView);
            ParcelListViewCommand = new RelayCommand<object>(ShowParcelListView);

        }
        private void ShowParcelListView(object obj)
        {
            new ParcelListView(new ParcelListViewModel(bl)).Show();
        }

        private void ShowStationListView(object obj)
        {
            new Station().Show();
        }

        public void ShowDroneListView(object obj)
        {
            new DroneListView(new DroneListViewModel(bl)).Show();
        }

        private void ShowCustomerListView(object obj)
        {
            new CustomerListView(new CustomerListViewModel(bl)).Show();
        }
    }
}
