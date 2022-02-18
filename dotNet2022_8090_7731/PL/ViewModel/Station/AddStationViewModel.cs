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
            try
            {

                    BlApi.BlFactory.GetBl().AddingBaseStation
                (new()
                {
                    Id = (int)station.Id,
                    NameStation = station.Name,
                    Location = new() { Longitude = (double)station.Longitude, Latitude = (double)station.Latitude },
                    NumAvailablePositions = (int)station.NumPositions
                }
                );
            }
            catch (BO.IdAlreadyExistsException)
            {
                MessageBox.Show("id is already exist in this list");
            }
            catch (BO.XMLFileLoadCreateException ex )
            {
                MessageBox.Show(ex.Message+",the xml file not exists");
            }
            Refresh.Invoke();
            switchView(Map(station));

        }

        private BO.Station Map(StationToAdd station)
        {
            BO.Location location = new() { Longitude = (double)station.Longitude, Latitude = (double)station.Latitude };
            return new BO.Station()
            {
                Id = (int)station.Id,
                NameStation = station.Name,
                Location = location,
                NumAvailablePositions = (int)station.NumPositions,
                LBL_ChargingDrone = null
            };
        }
    }
}
