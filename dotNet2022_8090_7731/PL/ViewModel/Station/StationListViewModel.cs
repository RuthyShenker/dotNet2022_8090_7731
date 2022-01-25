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
using System.Windows.Data;
using static PL.Model.Enum;

namespace PL.ViewModels
{
    public class StationListViewModel : ObservableBase 
    {
        int choosenNumPositions { get; set; }
        readonly BlApi.IBL bl;
        GroupOptionsForStationList groupBy;

        public ListCollectionView StationList { get; set; }
        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> ShowStationCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        //TODO update when it change
        public List<int> AvailablePositionsList { get; set; }

        public Array GroupOptions { get; set; } = Enum.GetValues(typeof(GroupOptionsForStationList));

        public StationListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            StationList = new(bl.GetStations().ToList());
            StationList.Filter = FilterCondition;
            AvailablePositionsList = bl.AvailableSlots().Select(station => station.AvailablePositions).Distinct().ToList();
            
            AddStationCommand = new RelayCommand<object>(AddingStation);
            ShowStationCommand = new RelayCommand<object>(ShowStation);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
        }

        public GroupOptionsForStationList GroupBy
        {
            get => groupBy;
            set
            {
               
                groupBy = value;
                StationList.GroupDescriptions.Clear();
                StationList.SortDescriptions.Clear();
                if (groupBy != GroupOptionsForStationList.All)
                {
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription(groupBy.ToString());
                    StationList.GroupDescriptions.Add(groupDescription);

                    SortDescription sortDescription = new SortDescription(groupBy.ToString(), ListSortDirection.Ascending);
                    StationList.SortDescriptions.Add(sortDescription);
                }

                // inner sort by Id
                StationList.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }
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
            //StationList = new(bl.AvailableSlots().ToList());
        }

        private void ShowStation(object sender)
        {
            var selectedStation = sender as StationToList;
            var blStation = bl.GetStation(selectedStation.Id);
            new StationView(bl, RefreshStationList, blStation)
                    .Show();
            //StationList = new(bl.AvailableSlots().ToList());

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
            StationList = new(bl.GetStations().ToList());
            //StationList.Refresh();
        }
    }
}
