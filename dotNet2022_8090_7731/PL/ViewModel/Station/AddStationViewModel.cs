using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PO;
namespace PL.ViewModels
{
    public class AddStationViewModel
    {
        public StationToAdd Station { get; set; } = new();
        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> CancelCommand { get; set; }

        public AddStationViewModel()
        {
            AddStationCommand = new RelayCommand<object>(AddStation, param => Station.Error == string.Empty);
            CancelCommand = new RelayCommand<object>(Functions.CloseWindow);
        }

        private void AddStation(object obj)
        {
            StationToAdd station = Station;
            try
            {
                BlApi.BlFactory.GetBl().AddingBaseStation
                (
                    (int)station.Id,
                    station.Name,
                    (double)station.Longitude,
                    (double)station.Latitude,
                    (int)station.NumPositions
                );
                Refresh.Invoke();
            }
            catch (Exception)
            {
                MessageBox.Show("id is already exist");
            }
        }
    }
}
