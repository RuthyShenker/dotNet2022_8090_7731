using BO;
using PL.View;
using System;
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
        private readonly BlApi.IBL bl;
        private ListCollectionView droneList;
        private WeightCategories weightSelectedItem;
        private DroneStatus statusSelectedItem;
        private GroupByDroneStatus currentGroup;

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
            if (obj is DroneToList droneToList)
            {
                if (WeightSelectedItem == default && StatusSelectedItem == default/*|| droneToList.DStatus == statusSelectedItem*/)
                    return true;
                else if (droneToList.DStatus == statusSelectedItem && droneToList.Weight == WeightSelectedItem)
                    return true;
            }
            return false;
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


        public WeightCategories WeightSelectedItem
        {
            get => weightSelectedItem;
            set
            {
                RaisePropertyChanged(nameof(WeightSelectedItem));
                weightSelectedItem = value;
                RefreshDrones();
            }
        }

        public DroneStatus StatusSelectedItem
        {
            get => statusSelectedItem;
            set
            {
                statusSelectedItem = value;
                RefreshDrones();
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