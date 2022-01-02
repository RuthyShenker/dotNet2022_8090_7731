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

        public RelayCommand<object> MouseDoubleCommand { get; set; }

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
            MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);

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

            var station = obj as EditStation;
            if ((station.Name is null or "") && (station.NumPositions is null or ""))
            {
                MessageBox.Show("two of them are empty");
            }
            else
            {
                bl.UpdatingStationDetails(station.Id, station.Name, (int)station.NumPositions);
                refreshStations();
                _ = CloseWindowCommand;
            }
            //TODO:
            // לחסום isenabled
            // האם להעביר את הבדיקות שאחד משתיהם ריק לשכבה הבאה ולעשות try catch?
        }

        private EditStation Map(BO.Station station)
        {
            return new EditStation(station.Id, station.NameStation, station.NumAvailablePositions,
               station.Location.Latitude, station.Location.Longitude, station.LBL_ChargingDrone);
        }

        private void Close_Window(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }
    }
}
