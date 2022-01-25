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
            this.refreshStations = refreshStations;
            Station = Map(station);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
            UpdateStationCommand = new RelayCommand<object>(UpdateStation, param => Station.Error == string.Empty);
            DeleteStationCommand = new RelayCommand<object>(DeleteStation, param => Station.ListChargingDrone.Count() == 0);
            ShowDroneCommand = new RelayCommand<object>(OpenSelectedDroneWindow);

            //ShowDroneInStationCommand = new RelayCommand<object>(MouseDoubleClick);
        }

        private void DeleteStation(object closeButton)
        {
            try
            {
                MessageBox.Show(bl.DeleteStation(Station.Id));
                refreshStations();
                CloseWindow(closeButton);
            }
            catch (IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void OpenSelectedDroneWindow(object obj)
        {
            var chargingDrone = obj as PO.ChargingDrone;
            var drone = bl.GetDrone(chargingDrone.DroneId);

            new DroneView(bl, refreshStations, drone).Show();
        }

        private void UpdateStation(object obj)
        {
            var station = obj as EditStation;

            //TODO: two feilds has to be full?
            bl.UpdatingStationDetails(station.Id, station.Name, (int)station.NumPositions);
            refreshStations();
            MessageBox.Show("Succseful Updating ");
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

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }
    }
}
