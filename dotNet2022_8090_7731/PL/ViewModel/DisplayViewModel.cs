using PL.View;
using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{

    public class DisplayViewModel
    {
        private readonly BlApi.IBL bl;

        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> CustomerListViewCommand { get; set; }
        public RelayCommand<object> DroneListViewCommand { get; set; }
        public RelayCommand<object> StationListViewCommand { get; set; }
        public RelayCommand<object> ParcelListViewCommand { get; set; }

        public DisplayViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
      
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
            new StationListView(new StationListViewModel(bl)).Show();
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
