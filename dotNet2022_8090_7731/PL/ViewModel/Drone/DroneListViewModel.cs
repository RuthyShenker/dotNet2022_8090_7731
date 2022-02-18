
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


namespace PL.ViewModels
{
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

        public DroneListViewModel(BlApi.IBL bl)
        {
            Refresh.DronesList += RefreshDronesList;

            this.bl = bl;

            DroneList = new(bl.GetDrones().MapListFromBLToPL().ToList());
            DroneList.Filter = FilterDrone;

            AddDroneCommand = new RelayCommand<object>(AddingDrone);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick, param => param != null);

            //GroupCommand = new RelayCommand<object>(GroupDrones);
            //droneList = Enumerable.Empty<DroneToList>();
            //FilterDroneListByCondition();
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
        /// מה הולך פה????????
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
                //RaisePropertyChanged(nameof(WeightSelectedItem));
                weightSelectedItem = value;
                DroneList.Filter = FilterDrone;
                //RefreshDrones();
            }
        }

        public object StatusSelectedItem
        {
            get => statusSelectedItem;
            set
            {
                statusSelectedItem = value;
                DroneList.Filter = FilterDrone;
                //RefreshDrones();
            }
        }

        private void AddingDrone(object sender)
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

        //TODOTODO:
        private void MouseDoubleClick(object sender)
        {
            var selectedDrone = sender as PO.DroneToList;
            var drone = bl.GetDrone(selectedDrone.Id);
            new DroneView(bl, drone)
                .Show();
        }

    }
}
#region
//private void button_Close_Click(object sender, RoutedEventArgs e)
//{
//    //Hide();
//}
//
//private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//{
//    //e.Cancel = true;
//    //MessageBox.Show($"You can press close:)",
//    //   "Canceled Action",
//    //   MessageBoxButton.OK,
//    //   MessageBoxImage.Information);
//}

//private void GroupDrones(object obj)
//{
//    groupBy = (GroupByDroneStatus)obj;
//    droneList.GroupDescriptions.Clear();
//    droneList.SortDescriptions.Clear();
//    if (groupBy != GroupByDroneStatus.Id)
//    {
//        PropertyGroupDescription groupDescription = new PropertyGroupDescription(groupBy.ToString());
//        //groupDescription.PropertyName = groupBy.ToString();
//        DroneList.GroupDescriptions.Add(groupDescription);
//        SortDescription sortDescription = new SortDescription(groupBy.ToString(), ListSortDirection.Ascending);
//        droneList.SortDescriptions.Add(sortDescription);
//    }
//    droneList.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
//}
//private string GroupByCurrentGroup()
//{

//    return groupBy switch
//    {
//        GroupByDroneStatus.Free => nameof(DroneToList.DStatus.Free),
//        GroupByDroneStatus.Maintenance => nameof(DroneToList.DStatus.Maintenance),
//        GroupByDroneStatus.Delivery => nameof(DroneToList.DStatus.Delivery),
//        GroupByDroneStatus.Id => "",
//        _ => null,
//    };
//}

//private void FilterDroneListByCondition()
//{
//    if (WeightSelectedItem == default && StatusSelectedItem == default)
//        DroneList =new( bl.GetDrones().ToList());
//    else if (StatusSelectedItem == default)
//        DroneList =new( bl.GetDrones(drone => drone.Weight == WeightSelectedItem).ToList());
//    else if (WeightSelectedItem == default)
//        DroneList =new( bl.GetDrones(drone => drone.DStatus == StatusSelectedItem).ToList());
//    else
//        DroneList =new( bl.GetDrones(drone =>
//        drone.DStatus == StatusSelectedItem
//        && drone.Weight == WeightSelectedItem).ToList());
//}
#endregion