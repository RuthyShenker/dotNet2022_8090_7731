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
using static PL.Extensions;

namespace PL.ViewModels
{
    public class StationListViewModel : INotify 
    {
        private object choosenNumPositions = "All";
        readonly BlApi.IBL bl;
        GroupOptionsForStationList groupBy;
        private ListCollectionView stationList;
        private IEnumerable availablePositionsList;

        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> ShowStationCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public Array GroupOptions { get; set; } = Enum.GetValues(typeof(GroupOptionsForStationList));

        public StationListViewModel(BlApi.IBL bl)
        {
            Refresh.StationsList += RefreshStationList;

            this.bl = bl;
            StationList = new(bl.GetStations().MapListFromBLToPL().ToList());
            StationList.Filter = FilterCondition;
            AvailablePositions();

            AddStationCommand = new RelayCommand<object>(AddingStation);
            ShowStationCommand = new RelayCommand<object>(ShowStation);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
        }

        private void AvailablePositions()
        {
            AvailablePositionsList = new List<object>() { "All" }
            .Union(
                bl.AvailableSlots()
                .Select(station => station.AvailablePositions)
                .Distinct()
                .Cast<object>());
        }

        public ListCollectionView StationList
        {
            get => stationList;
            set
            {
                stationList = value;
                RaisePropertyChanged(nameof(StationList));
            }
        }

        public IEnumerable AvailablePositionsList
        {
            get => availablePositionsList;
            set
            {
                availablePositionsList = value;
                RaisePropertyChanged(nameof(AvailablePositionsList));
            }
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
                    PropertyGroupDescription groupDescription = new(groupBy.ToString());
                    StationList.GroupDescriptions.Add(groupDescription);

                    SortDescription sortDescription = new (groupBy.ToString(), ListSortDirection.Ascending);
                    StationList.SortDescriptions.Add(sortDescription);
                }

                // inner sort by Id
                StationList.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }
        }

        private void AddingStation(object sender)
        {
            new StationView(bl)
                      .Show();
            
        }

        private void ShowStation(object sender)
        {
            var selectedStation = sender as PO.StationToList;
            var blStation = bl.GetStation(selectedStation.Id);
            new StationView(bl, /*RefreshStationList,*/ blStation)
                    .Show();
            //StationList = new(bl.AvailableSlots().ToList());

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
            PO.StationToList station = obj as PO.StationToList;
            return choosenNumPositions is null or "All" || station.AvailablePositions.Equals(choosenNumPositions);
        }

        private void RefreshStationList()
        {
            StationList = new(bl.GetStations().MapListFromBLToPL().ToList());
            AvailablePositions();

            // keep filter and group status
            FilterList = choosenNumPositions;
            GroupBy = groupBy;

            //StationList.Refresh();
        }
    }
}
