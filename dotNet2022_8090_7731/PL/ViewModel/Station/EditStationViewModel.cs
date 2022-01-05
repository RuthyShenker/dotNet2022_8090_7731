using BO;
using PL.View;
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
    public class EditStationViewModel
    {
        Drone Drone;
        BlApi.IBL bl;
        Action refreshStations;
        //EditStation station;
        public EditStation Station { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> UpdateStationCommand { get; set; }
        public RelayCommand<object> DeleteStationCommand { get; set; }

        public RelayCommand<object> ShowDroneCommand { get; set; }

        //public RelayCommand<object> ShowDroneInStationCommand { get; set; }
        //public RelayCommand<object> ShowParcelOfCustomerCommand { get; set; }

        public EditStationViewModel(BlApi.IBL bl, BO.Station station, Action refreshStations)
        {
            this.bl = bl;
            Station = Map(station);
            this.refreshStations = refreshStations;
            CloseWindowCommand = new RelayCommand<object>(Close_Window);
            UpdateStationCommand = new RelayCommand<object>(UpdateStation, param => Station.Error == string.Empty);
            DeleteStationCommand = new RelayCommand<object>(DeleteStation, param => Station.ListChargingDrone.Count() == 0);
            ShowDroneCommand = new RelayCommand<object>(ShowDrone);

            //ShowDroneInStationCommand = new RelayCommand<object>(MouseDoubleClick);
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

        private void ShowDrone(object obj)
        {
            var chargingDrone = obj as PO.ChargingDrone;
            var drone = bl.GetDrone(chargingDrone.DroneId);
            new DroneView(bl, null, drone)
              .Show();
        }

        private void UpdateStation(object obj)
        {
            var station = obj as EditStation;

            bl.UpdatingStationDetails(station.Id, station.Name, (int)station.NumPositions);
            //refreshStations();
            MessageBox.Show("Update Succseful");
            refreshStations();
            //TODO: two feilds has to be full?
        }

        private static EditStation Map(BO.Station station)
        {
            return new EditStation()
            {
                Id = station.Id,
                Name = station.NameStation,
                NumPositions = station.NumAvailablePositions,
                Location = new PO.Location(station.Location.Latitude, station.Location.Longitude),
                ListChargingDrone = station.LBL_ChargingDrone.Select(position => new PO.ChargingDrone(position.DroneId, position.BatteryStatus))
            };
        }

        private void Close_Window(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }
    }
}
