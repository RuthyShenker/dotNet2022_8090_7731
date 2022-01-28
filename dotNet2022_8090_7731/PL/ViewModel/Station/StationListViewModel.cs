using BO;
using PL.View;
using PO;
using System;
using System.Collections;
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
    public delegate void DelEventHandler();

    public class StationListViewModel : ObservableBase 
    {
        private object choosenNumPositions { get; set; } = "All";
        readonly BlApi.IBL bl;
        GroupOptionsForStationList groupBy;

        public ListCollectionView StationList { get; set; }
        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> ShowStationCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        //TODO update when it change
        public IEnumerable AvailablePositionsList { get; set; }

        public Array GroupOptions { get; set; } = Enum.GetValues(typeof(GroupOptionsForStationList));

        public StationListViewModel(BlApi.IBL bl)
        {
            Refresh.StationsList = RefreshStationList;

            this.bl = bl;
            StationList = new(bl.GetStations().ToList());
            StationList.Filter = FilterCondition;
            AvailablePositions();

            AddStationCommand = new RelayCommand<object>(AddingStation);
            ShowStationCommand = new RelayCommand<object>(ShowStation);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
        }

        private void AvailablePositions()
        {
            AvailablePositionsList = new List<object>() { "All" }.Union(bl.AvailableSlots().Select(station => station.AvailablePositions).Distinct().Cast<object>());
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
            //if (bl.AvailableSlots().Select(slot => slot.Id).Count() > 0)
            //{
                new StationView(bl, RefreshStationList)
                    .Show();
            //}
            //else
            //{
            //    MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
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

        public object FilterList
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
            return choosenNumPositions is null or "All" || station.AvailablePositions.Equals(choosenNumPositions);
        }

        private void RefreshStationList()
        {
            StationList = new(bl.GetStations().ToList());
            AvailablePositions();

            MessageBox.Show("Test");
            //StationList.Refresh();
        }
    }
}
