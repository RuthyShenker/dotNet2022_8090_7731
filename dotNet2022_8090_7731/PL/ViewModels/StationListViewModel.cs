using BO;
using PL.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL.ViewModels
{
    public class StationListViewModel 
    {
        private int choosenNumPositions { get; set; }
        private readonly BlApi.IBL bl;
        //public IEnumerable<StationToList> StationList { get; set; }
        public ListCollectionView StationList { get; set; }
        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> ShowStationCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        //TODO update when it change
        public List<int> AvailablePositionsList { get; set; }

        public StationListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            StationList = new(bl.AvailableSlots().ToList());
            StationList.Filter = FilterCondition;
            AvailablePositionsList = bl.AvailableSlots().Select(station => station.AvailablePositions).Distinct().ToList();
            
            AddStationCommand = new RelayCommand<object>(AddingStation);
            ShowStationCommand = new RelayCommand<object>(ShowStation);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
        }

        private void AddingStation(object sender)
        {
            if (bl.AvailableSlots().Select(slot => slot.Id).Count() > 0)
            {
                new StationView(bl, null)
                    .Show();
            }
            else
            {
                MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowStation(object sender)
        {
            var selectedStation = sender as StationToList;
            var blStation = bl.GetStation(selectedStation.Id);
            new StationView(bl, RefreshStationList, blStation)
                    .Show();
        }

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        public int FilterList
        {
            get => choosenNumPositions;
            set
            {
                choosenNumPositions = value;
                StationList.Filter = FilterCondition;
            }
        }

        private bool FilterCondition(object obj)
        {
            StationToList station = obj as StationToList;
            return choosenNumPositions == 0 || station.AvailablePositions == choosenNumPositions;
        }

        private void RefreshStationList()
        {
            StationList.Refresh();
        }
    }
}
