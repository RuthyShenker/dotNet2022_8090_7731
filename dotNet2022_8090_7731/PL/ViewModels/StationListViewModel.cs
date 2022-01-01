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
        public IEnumerable<StationToList> stationList;
        public RelayCommand<object> AddStationCommand { get; set; }
            public RelayCommand<object> MouseDoubleCommand { get; set; }
            public RelayCommand<object> CloseWindowCommand { get; set; }

            public StationListViewModel(BlApi.IBL bl)
            {
                this.bl = bl;
            stationList = Enumerable.Empty<StationToList>();
            RefreshStationList();
            AddStationCommand = new RelayCommand<object>(AddingStation);
                MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);
                CloseWindowCommand = new RelayCommand<object>(CloseWindow);
            }

        public IEnumerable<StationToList> StationList
            {
            get => stationList;
                set
                {
                stationList = value;
                RaisePropertyChanged(nameof(stationList));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void RaisePropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private void MouseDoubleClick(object sender)
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
