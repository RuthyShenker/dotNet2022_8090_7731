
using PL.View;
using PO;
using System;
using System.Linq;
using System.Windows;
using static PL.Extensions;

namespace PL.ViewModels
{
    public class EditStationViewModel : INotify
    {
        readonly BlApi.IBL bl;
        //readonly Action refreshStations;
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

            Station = Map(station);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            UpdateStationCommand = new RelayCommand<object>(UpdateStation, param => Station.Error == string.Empty);
            DeleteStationCommand = new RelayCommand<object>(DeleteStation, param => Station.ListChargingDrone == default);
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
            try
            {
                if (bl.GetStations().FirstOrDefault(s => s.Id == Station.Id) != default)
                {
                    Station = Map(bl.GetStation(station.Id));
                }
            }
            catch (BO.XMLFileLoadCreateException exception)
            {

                ShowXMLExceptionMessage(exception.Message);

            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);

            }
        }

        private void DeleteStation(object closeButton)
        {
            if (Extensions.WorkerTurnOn()) return;

            try
            {
                MessageBox.Show(bl.DeleteStation(Station.Id));
                Refresh.Invoke();

                Functions.CloseWindow(closeButton);
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);

            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                ShowXMLExceptionMessage(exception.Message);

            }
        }

        private void OpenSelectedDroneWindow(object obj)
        {

            var chargingDrone = obj as PO.ChargingDrone;
            try
            {
                var drone = bl.GetDrone(chargingDrone.DroneId);
                new DroneView(bl, drone).Show();
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                ShowXMLExceptionMessage(exception.Message);
            }

        }

        private void UpdateStation(object obj)
        {
            if (Extensions.WorkerTurnOn()) return;

            var station = obj as EditStation;

            //TODO: two feilds has to be full?
            try
            {
                bl.UpdatingStationDetails(station.Id, station.Name, (int)station.NumPositions);
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);


            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                ShowXMLExceptionMessage(exception.Message);

            }
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
                Location = new PO.Location()
                {
                    Latitude = station.Location.Latitude,
                    Longitude = station.Location.Longitude
                },
                ListChargingDrone = station.LBL_ChargingDrone?.Select(position => new PO.ChargingDrone() { DroneId = position.DroneId, BatteryStatus = position.BatteryStatus })
            };
        }
    }
}
