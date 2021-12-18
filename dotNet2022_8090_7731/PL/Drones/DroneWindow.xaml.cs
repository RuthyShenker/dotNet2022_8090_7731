
using IBL.BO;
using PL.Drones;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PL.Drones
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {

        private IBL.IBL bl;
        Action refreshDroneList;

        public DroneWindow(IBL.IBL bl, Action initializeDrones)
        {
            this.bl = bl;
            refreshDroneList = initializeDrones;
            InitializeComponent();
            DroneView.DataContext = new AddNewDroneView(bl, initializeDrones,this.Close);
        }
      
        public DroneWindow(IBL.IBL bl, Action initializeDrones, Drone selectedDrone)
        {
            InitializeComponent();
            this.bl = bl;
            refreshDroneList = initializeDrones;
            DroneView.DataContext= new EditDroneView(bl, initializeDrones, selectedDrone);
        }

        
        //private void ChangeVisibility(Visibility visibility, params StackPanel[] stackPanels)
        //{
        //    foreach (StackPanel item in stackPanels)
        //    {
        //        item.Visibility = visibility;
        //    }
        //}

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}


