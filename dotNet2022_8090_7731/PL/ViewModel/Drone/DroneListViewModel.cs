using BO;
using PL.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using static PL.Model.Enum;

namespace PL.ViewModels
{
    public class DroneListViewModel : INotifyPropertyChanged
    {
        readonly BlApi.IBL bl;
        ListCollectionView droneList;
        object weightSelectedItem="Default";
        object statusSelectedItem="Default";

        GroupByDroneStatus currentGroup;

        public IEnumerable WeightCategoriesEnum { get; } = new List<object>() { "Defalut" }.Union(Enum.GetValues(typeof(WeightCategories)).Cast<object>());
        public IEnumerable DroneStatusEnum { get; } = new List<object>() { "Defalut" }.Union(Enum.GetValues(typeof(DroneStatus)).Cast<object>());
        public Array GroupOptions { get; set; }
        public RelayCommand<object> AddDroneCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        //public RelayCommand<object> GroupCommand { get; set; }

        public DroneListViewModel(BlApi.IBL bl)
        {

            this.bl = BlApi.BlFactory.GetBl();/* bl;*/
            //droneList = Enumerable.Empty<DroneToList>();
            //FilterDroneListByCondition();

            AddDroneCommand = new RelayCommand<object>(AddingDrone);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
            MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);
            //GroupCommand = new RelayCommand<object>(GroupDrones);
            var list = new ObservableCollection<DroneToList>(bl.GetDrones());
            DroneList = new ListCollectionView(list);
            GroupOptions = Enum.GetValues(typeof(GroupByDroneStatus));
            DroneList.Filter = FilterDrone;
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
            //if (obj is DroneToList droneToList)
            //{
                //if ((WeightSelectedItem == null && StatusSelectedItem == null) || (WeightSelectedItem == "Default" && StatusSelectedItem == "Default"))//*|| droneToList.DStatus == statusSelectedItem*/
                //    return true;
                //else if (WeightSelectedItem == null && (statusSelectedItem == "Default" || droneToList.DStatus == (DroneStatus)statusSelectedItem))
                //    return true;
                //else if ((WeightSelectedItem == "Default" || droneToList.Weight == (WeightCategories)WeightSelectedItem) && statusSelectedItem == null)
                //    return true;
                //else if ((statusSelectedItem == "Default" || droneToList.DStatus == (DroneStatus)statusSelectedItem) && (weightSelectedItem == "Default" || droneToList.Weight == (WeightCategories)weightSelectedItem))
                //    return true;
            //}

            if (obj is DroneToList droneToList)
            {
               if(Enum.IsDefined(typeof(WeightCategories),WeightSelectedItem)&& Enum.IsDefined(typeof(DroneStatus), statusSelectedItem))
                   return (WeightCategories)WeightSelectedItem == droneToList.Weight&& (DroneStatus)statusSelectedItem == droneToList.DStatus;
               if (Enum.IsDefined(typeof(WeightCategories), WeightSelectedItem))
                   return (WeightCategories)WeightSelectedItem == droneToList.Weight;
               if (Enum.IsDefined(typeof(DroneStatus), statusSelectedItem))
                   return (DroneStatus)statusSelectedItem == droneToList.DStatus;
            }
            return true;
        }

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
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription(currentGroup.ToString());
                    DroneList.GroupDescriptions.Add(groupDescription);

                    SortDescription sortDescription = new SortDescription(currentGroup.ToString(), ListSortDirection.Ascending);
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //GroupDrones = currentGroup;
        }

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        private void AddingDrone(object sender)
        {
            if (bl.AvailableSlots().Select(slot => slot.Id).Any())
            {
                new DroneView(/*bl,*/RefreshDrones).Show();
            }
            else
            {
                MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshDrones()
        {
            DroneList = new(bl.GetDrones().ToList());
            GroupDrones = currentGroup;
        }

        //TODOTODO:
        private void MouseDoubleClick(object sender)
        {
            var selectedDrone = sender as DroneToList;
            var drone = bl.GetDrone(selectedDrone.Id);
            new DroneView(bl, RefreshDrones, drone)
                .Show();
        }

    }
}
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