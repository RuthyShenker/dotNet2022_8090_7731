using PL.Drones;
using PL.View;
using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //BlApi.IBL bl;
        //public RelayCommand<object> CustomerListViewCommand;
        //public RelayCommand<object> DroneListViewCommand;
        //public RelayCommand<object> StationListViewCommand;
        //public RelayCommand<object> ParcelListViewCommand;
        public MainWindow()
        {
            //bl = BlApi.BlFactory.GetBl();
            new DisplayView(new DisplayViewModel()).Show();
            InitializeComponent();
            //CustomerListViewCommand = new RelayCommand<object>(ShowCustomerListView);
            //DroneListViewCommand = new RelayCommand<object>(ShowDroneListView);
            //StationListViewCommand = new RelayCommand<object>(ShowStationListView);
            //ParcelListViewCommand = new RelayCommand<object>(ShowParcelListView);

        }

        //private void ShowParcelListView(object obj)
        //{
        //    new ParcelListView(new ParcelListViewModel(bl)).Show();
        //}

        //private void ShowStationListView(object obj)
        //{
        //    new Station().Show();
        //}

        //public void ShowDroneListView(object obj)
        //{
        //    new DroneListView(new DroneListViewModel(bl)).Show();
        //}

        //private void ShowCustomerListView(object obj)
        //{
        //    new CustomerListView(new CustomerListViewModel(bl)).Show();
        //}
    }
}
