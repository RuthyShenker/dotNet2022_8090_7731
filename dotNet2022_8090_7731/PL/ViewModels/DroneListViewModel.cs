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
    public class DroneListViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        WeightCategories weightSelectedItem;
        DroneStatus statusSelectedItem;
        public IEnumerable<DroneToList> droneList;
        public RelayCommand<object> AddDroneCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public DroneListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            droneList = Enumerable.Empty<DroneToList>();
            FilterDroneListByCondition();
            AddDroneCommand = new RelayCommand<object>(AddingDrone);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
            MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);
        }

        private void FilterDroneListByCondition()
        {
            if (WeightSelectedItem == default && StatusSelectedItem == default)
                DroneList = bl.GetDrones();
            else if (StatusSelectedItem == default)
                DroneList = bl.GetDrones(drone => drone.Weight == WeightSelectedItem);
            else if (WeightSelectedItem == default)
                DroneList = bl.GetDrones(drone => drone.DStatus == StatusSelectedItem);
            else
                DroneList = bl.GetDrones(drone =>
                drone.DStatus == StatusSelectedItem
                && drone.Weight == WeightSelectedItem);
        }
        public IEnumerable<DroneToList> DroneList
        {
            get => droneList;
            set
            {
                droneList = value;
                RaisePropertyChanged(nameof(DroneList));
            }
        }
        public WeightCategories WeightSelectedItem
        {
            get => weightSelectedItem;
            set
            {
                weightSelectedItem = value;
                FilterDroneListByCondition();
            }
        }
        public DroneStatus StatusSelectedItem
        {
            get => statusSelectedItem;
            set
            {
                statusSelectedItem = value;
                FilterDroneListByCondition();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        private void AddingDrone(object sender)
        {
            if (bl.AvailableSlots().Select(slot => slot.Id).Count() > 0)
            {
                //var viewModel = new AddDroneViewModel(bl,FilterDroneListByCondition);
                new DroneView(bl,FilterDroneListByCondition).Show();
                //new DroneView(/*bl,*/FilterDroneListByCondition).Show();
            }
            else
            {
                MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void button_Close_Click(object sender, RoutedEventArgs e)
        //{
        //    //Hide();
        //}

        //TODOTODO:
        private void MouseDoubleClick(object sender)
        {
            var selectedDrone = sender as DroneToList;
            var drone = bl.GetDrone(selectedDrone.Id);
            new DroneView(bl, FilterDroneListByCondition, drone)
                .Show();
        }

        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    //e.Cancel = true;
        //    //MessageBox.Show($"You can press close:)",
        //    //   "Canceled Action",
        //    //   MessageBoxButton.OK,
        //    //   MessageBoxImage.Information);
        //}
    }
}
