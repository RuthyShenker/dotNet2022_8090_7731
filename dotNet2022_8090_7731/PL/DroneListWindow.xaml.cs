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
            DroneListView.DataContext = bl.GetDrones();
            DroneWeights.DataContext = Enum.GetValues(typeof(WeightCategories));
            DroneStatuses.DataContext = Enum.GetValues(typeof(DroneStatus));
            
        }

        private void DroneWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DroneWeights.SelectedItem == null)
            {
                DroneListView.ItemsSource = bl.GetDrones();
            }
            else
            {
                WeightCategories weight = (WeightCategories)DroneWeights.SelectedItem;
                DroneListView.ItemsSource = bl.GetDrones(drone => drone.Weight==weight);
            }
        }

        private void DroneStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (DroneStatuses.SelectedItem == null)
            {
                DroneListView.ItemsSource = bl.GetDrones();
            }
            else
            {
                DroneStatus status = (DroneStatus)DroneStatuses.SelectedItem;
                DroneListView.ItemsSource = bl.GetDrones(drone=>drone.DStatus== status);
            }
        }

        private void button_AddingDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show($"{sender as ListView}");
            new DroneWindow(bl,sender as DroneToList).Show();
        }
    }
}
