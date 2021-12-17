using BO;
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

namespace PL
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
            {
                DroneListView.DataContext = bl.GetDrones();
            }
            else if (DroneStatuses.SelectedItem == null)
            {
                WeightCategories weight = (WeightCategories)DroneWeights.SelectedItem;
                DroneListView.DataContext = bl.GetDrones(drone => drone.Weight == weight);
            }
            else if (DroneWeights.SelectedItem == null)
            {
                DroneStatus status = (DroneStatus)DroneStatuses.SelectedItem;
                DroneListView.DataContext = bl.GetDrones(drone => drone.DStatus == status);
            }
            else
            {
                WeightCategories weight = (WeightCategories)DroneWeights.SelectedItem;
                DroneStatus status = (DroneStatus)DroneStatuses.SelectedItem;
                DroneListView.DataContext = bl.GetDrones(drone => drone.DStatus == status && drone.Weight == weight);
            }
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
            this.Close();
        }

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedDrone = (e.OriginalSource as FrameworkElement).DataContext as DroneToList;
            new DroneWindow(bl, FilterDroneListByCondition, selectedDrone)
                .Show();
        }


    }
}
