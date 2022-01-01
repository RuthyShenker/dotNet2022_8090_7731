using BO;
using PO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class EditStationViewModel : INotifyPropertyChanged
    {
        Drone Drone;
        BlApi.IBL bl;
        Action refreshStations;
        EditStation station;
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> UpdateStationCommand { get; set; }
        public RelayCommand<object> DeleteStationCommand { get; set; }

        public RelayCommand<object> ShowDroneInStationCommand { get; set; }
        //public RelayCommand<object> ShowParcelOfCustomerCommand { get; set; }

        public EditStationViewModel(BlApi.IBL bl, BO.Station station, Action refreshStations)
        {
            this.bl = bl;
            Station = Map(station);
            this.refreshStations = refreshStations;
            CloseWindowCommand = new RelayCommand<object>(Close_Window);
            UpdateStationCommand = new RelayCommand<object>(UpdateStation);
            DeleteStationCommand = new RelayCommand<object>(DeleteStation);
            ShowDroneInStationCommand = new RelayCommand<object>(MouseDoubleClick);
        }

        private void DeleteStation(object obj)
        {
            try
            {
                var station = obj as BO.Station;
                MessageBox.Show(bl.DeleteStation(station.Id));
                refreshStations();
            }
            catch (IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void MouseDoubleClick(object obj)
        {
            MessageBox.Show("herehhhh");
            var station = obj as BO.Station;
            var dronesList = station.LBL_ChargingDrone;
            //var bldrone = bl.GetParcel(parcel.Id);
            //new ParcelView(bl, refreshParcelList, blParcel).Show();
        }

        private void UpdateStation(object obj)
        {
            try
            {
                var station = obj as BO.Station;
                bl.UpdatingStationDetails(station.Id, station.NameStation, station.NumAvailablePositions);
                //TODO:
                //MessageBox.Show("",,,);  ?
                refreshStations();
            }
            catch (IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public EditStation Station
        {
            get => station;
            private set
            {
                station = value;
                RaisePropertyChanged(nameof(Station));
            }
        }

        private EditStation Map(BO.Station station)
        {
            return new EditStation(station.Id, station.NameStation, station.NumAvailablePositions, 
               new PO.Location( station.Location), station.LBL_ChargingDrone);
        }
      
        private void Close_Window(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}