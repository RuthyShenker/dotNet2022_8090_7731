
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Drones
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBL bl;
        public DroneListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            FilterDroneListByCondition();

            DroneWeights.DataContext = Enum.GetValues(typeof(WeightCategories));
            DroneStatuses.DataContext = Enum.GetValues(typeof(DroneStatus));

        }


        private void DroneWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterDroneListByCondition();
        }

        private void FilterDroneListByCondition()
        {
            if (DroneWeights.SelectedItem == null && DroneStatuses.SelectedItem == null)
                DroneListView.DataContext = bl.GetDrones();
            else if (DroneStatuses.SelectedItem == null)
                DroneListView.DataContext = bl.GetDrones(drone => drone.Weight == (WeightCategories)DroneWeights.SelectedItem);
            else if (DroneWeights.SelectedItem == null)
                DroneListView.DataContext = bl.GetDrones(drone => drone.DStatus == (DroneStatus)DroneStatuses.SelectedItem);
            else
                DroneListView.DataContext = bl.GetDrones(drone =>
                drone.DStatus == (DroneStatus)DroneStatuses.SelectedItem
                && drone.Weight == (WeightCategories)DroneWeights.SelectedItem);
        }

        private void DroneStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            FilterDroneListByCondition();
        }

        private void button_AddingDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, FilterDroneListByCondition)
                .Show();
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedDrone = (sender as ContentControl).DataContext as DroneToList;
            var drone = bl.GetDrone(selectedDrone.Id);
            new DroneWindow(bl, FilterDroneListByCondition, drone)
                .Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
