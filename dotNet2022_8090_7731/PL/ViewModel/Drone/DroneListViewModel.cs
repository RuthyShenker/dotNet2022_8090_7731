
using PL.View;
using PO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using static PL.Extensions;

namespace PL.ViewModels
{
    /// <summary>
    ///  public class DroneListViewModel : INotify
    /// </summary>
    public class DroneListViewModel : INotify
    {
        private readonly BlApi.IBL bl;
        private ListCollectionView droneList;
        private object weightSelectedItem = "Default";
        private object statusSelectedItem = "Default";

        GroupByDroneStatus currentGroup;

        public IEnumerable WeightCategoriesEnum { get; } = new List<object>() { "Defalut" }.Union(Enum.GetValues(typeof(PO.WeightCategories)).Cast<object>());
        public IEnumerable DroneStatusEnum { get; } = new List<object>() { "Defalut" }.Union(Enum.GetValues(typeof(PO.DroneStatus)).Cast<object>());
        public Array GroupOptions { get; set; } = Enum.GetValues(typeof(GroupByDroneStatus));
        public RelayCommand<object> AddDroneCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        //public RelayCommand<object> GroupCommand { get; set; }

        /// <summary>
        /// A constructor of DroneListViewModel that gets bl.
        /// </summary>
        /// <param name="bl"></param>
        public DroneListViewModel(BlApi.IBL bl)
        {
            Refresh.DronesList += RefreshDronesList;

            this.bl = bl;

            DroneList = new(bl.GetDrones().MapListFromBLToPL().ToList());
            DroneList.Filter = FilterDrone;

            AddDroneCommand = new RelayCommand<object>(AddingDrone);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);
        }

        public ListCollectionView DroneList
        {
            get => droneList;
            set
            {
                droneList = value;
                RaisePropertyChanged(nameof(DroneList));
            }
        }

        /// <summary>
        /// A function that filter drones.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FilterDrone(object obj)
        {
            if (obj is PO.DroneToList droneToList)
            {
                if (Enum.IsDefined(typeof(PO.WeightCategories), WeightSelectedItem) && Enum.IsDefined(typeof(PO.DroneStatus), statusSelectedItem))
                    return (PO.WeightCategories)WeightSelectedItem == droneToList.Weight && (PO.DroneStatus)statusSelectedItem == droneToList.DStatus;
                if (Enum.IsDefined(typeof(PO.WeightCategories), WeightSelectedItem))
                    return (PO.WeightCategories)WeightSelectedItem == droneToList.Weight;
                if (Enum.IsDefined(typeof(PO.DroneStatus), statusSelectedItem))
                    return (PO.DroneStatus)statusSelectedItem == droneToList.DStatus;
            }
            return true;
        }

        /// <summary>
        /// Group drones by selected group, always sort by Id
        /// </summary>
        public GroupByDroneStatus GroupDrones
        {
            get => currentGroup;
            set
            {
                currentGroup = value;
                droneList.GroupDescriptions.Clear();
                droneList.SortDescriptions.Clear();
                if (currentGroup != GroupByDroneStatus.Id)
                {
                    PropertyGroupDescription groupDescription = new(currentGroup.ToString());
                    DroneList.GroupDescriptions.Add(groupDescription);

                    SortDescription sortDescription = new(currentGroup.ToString(), ListSortDirection.Ascending);
                    droneList.SortDescriptions.Add(sortDescription);
                }
                droneList.SortDescriptions.Add(new SortDescription(nameof(GroupByDroneStatus.Id), ListSortDirection.Ascending));
            }
        }

        public object WeightSelectedItem
        {
            get => weightSelectedItem;
            set
            {
                weightSelectedItem = value;
                DroneList.Filter = FilterDrone;
            }
        }

        public object StatusSelectedItem
        {
            get => statusSelectedItem;
            set
            {
                statusSelectedItem = value;
                DroneList.Filter = FilterDrone;
            }
        }

        /// <summary>
        /// A function that adds a drone .
        /// </summary>
        /// <param name="sender"></param>
        private void AddingDrone(object sender)
        {
            try
            {
                if (bl.AvailableSlots().Select(slot => slot.Id).Any())
                {
                    new DroneView(/*bl,*//*RefreshDrones*/).Show();
                }
                else
                {
                    MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }
        }

        /// <summary>
        /// A function that refreshes the Drone List.
        /// </summary>
        private void RefreshDronesList()
        {
            lock (bl)
            {
                DroneList = new(bl.GetDrones().MapListFromBLToPL().ToList());

                // keep group and filter status
                GroupDrones = currentGroup;
                DroneList.Filter = FilterDrone;
            }
        }

        /// <summary>
        /// A function that opens specific drone.
        /// </summary>
        /// <param name="sender"></param>
        private void MouseDoubleClick(object sender)
        {
            if (sender == null) return;

            var selectedDrone = sender as PO.DroneToList;
            try
            {
                var drone = bl.GetDrone(selectedDrone.Id);
                new DroneView(bl, drone)
                    .Show();
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }
        }
    }
}
