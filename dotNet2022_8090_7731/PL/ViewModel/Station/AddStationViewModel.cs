using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;
using PO;
namespace PL.ViewModels
{
    public class AddStationViewModel
    {
        readonly Action<BO.Station> switchView;
        public StationToAdd Station { get; set; } = new();
        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> CancelCommand { get; set; }

        public AddStationViewModel(Action<BO.Station> switchView)
        {
            this.switchView = switchView;
            AddStationCommand = new RelayCommand<object>(AddStation, param => Station.Error == string.Empty);
            CancelCommand = new RelayCommand<object>(Functions.CloseWindow);
        }

        private void AddStation(object obj)
        {
            StationToAdd station = Station;
            //try
            //{

                BlApi.BlFactory.GetBl().AddingBaseStation
                (
                    (int)station.Id,
                    station.Name,
                    (double)station.Longitude,
                    (double)station.Latitude,
                    (int)station.NumPositions
                );
                Refresh.Invoke();
                switchView(Map(station));
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("id is already exist");
            //}
        }

        private Station Map(StationToAdd station)
        {
            BO.Location location = new((double)station.Longitude, (double)station.Latitude);
            return new Station((int)station.Id, station.Name,location,(int)station.NumPositions);
        }
    }
}
