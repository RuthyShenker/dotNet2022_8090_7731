using BO;
using PL.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class StationListViewModel : INotifyPropertyChanged
    {
        private readonly BlApi.IBL bl;
        public IEnumerable<StationToList> StationList { get; set; }
        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> ShowStationCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> SortListCommand { get; set; }
        public List<int> AvailablePositionsList { get; set; }

        public StationListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            StationList = Enumerable.Empty<StationToList>();
            AvailablePositionsList = bl.AvailableSlots().Select(station => station.AvailablePositions).Distinct().ToList();
            
            RefreshStationList();
            AddStationCommand = new RelayCommand<object>(AddingStation);
            ShowStationCommand = new RelayCommand<object>(ShowStation);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
            SortListCommand = new RelayCommand<object>(SortList);
        }

        private void SortList(object num)
        {
            StationList = bl.AvailableSlots((int)num);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ShowStation(object sender)
        {
            var selectedStation = sender as StationToList;
            var blStation = bl.GetStation(selectedStation.Id);
            new StationView(bl, RefreshStationList, blStation)
                    .Show();
        }

        private void AddingStation(object sender)
        {
            if (bl.AvailableSlots().Select(slot => slot.Id).Count() > 0)
            {
                new StationView(bl, RefreshStationList)
                    .Show();
            }
            else
            {
                MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshStationList()
        {
            StationList = bl.GetStations();
        }

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }
    }
}
