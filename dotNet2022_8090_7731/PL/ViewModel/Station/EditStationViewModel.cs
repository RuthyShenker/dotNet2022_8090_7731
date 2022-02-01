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
    public class EditStationViewModel : INotify
    {
        BlApi.IBL bl;
        Action refreshStations;
        //EditStation station;
        EditStation station;
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> UpdateStationCommand { get; set; }
        public RelayCommand<object> DeleteStationCommand { get; set; }

        public RelayCommand<object> ShowDroneCommand { get; set; }

        //public RelayCommand<object> ShowDroneInStationCommand { get; set; }
        //public RelayCommand<object> ShowParcelOfCustomerCommand { get; set; }

        public EditStationViewModel(BlApi.IBL bl, BO.Station station)
        {
            this.bl = bl;
            Refresh.Station += RefreshStation;

            //this.refreshStations = refreshStations;
            Station = Map(station);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            UpdateStationCommand = new RelayCommand<object>(UpdateStation, param => Station.Error == string.Empty);
            DeleteStationCommand = new RelayCommand<object>(DeleteStation, param => Station.ListChargingDrone.Count() == 0);
            ShowDroneCommand = new RelayCommand<object>(OpenSelectedDroneWindow);

            //ShowDroneInStationCommand = new RelayCommand<object>(MouseDoubleClick);
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

        private void RefreshStation()
        {
            //refreshParcels();
            Station = Map(bl.GetStation(station.Id));
        }

        private void DeleteStation(object closeButton)
        {
            try
            {
                MessageBox.Show(bl.DeleteStation(Station.Id));
                Refresh.Invoke();

                Functions.CloseWindow(closeButton);
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

            new DroneView(bl, drone).Show();
        }

        private void UpdateStation(object obj)
        {
            var station = obj as EditStation;

            //TODO: two feilds has to be full?
            bl.UpdatingStationDetails(station.Id, station.Name, (int)station.NumPositions);
            Refresh.Invoke();
            MessageBox.Show("Succseful Updating ");
        }

        private static EditStation Map(BO.Station station)
        {
            //MessageBox.Show(station.Location.Longitude.ToString());
            return new EditStation()
            {
                Id = station.Id,
                Name = station.NameStation,
                NumPositions = station.NumAvailablePositions,
                Location = new PO.Location(23, 23),
                ListChargingDrone = station.LBL_ChargingDrone.Select(position => new PO.ChargingDrone(position.DroneId, position.BatteryStatus))
            };
        }
    }
}
