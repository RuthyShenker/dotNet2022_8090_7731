
using BO;
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

        private BlApi.IBL bl;
        Action refreshDroneList;

        public DroneWindow(BlApi.IBL bl, Action initializeDrones)
        {
            this.bl = bl;
            refreshDroneList = initializeDrones;
            InitializeComponent();
          //  DroneView.DataContext = new AddNewDroneView(bl, initializeDrones,Close);
            DroneView.DataContext = new DroneView();
        }
      
        public DroneWindow(BlApi.IBL bl, Action initializeDrones, Drone selectedDrone)
        {
            InitializeComponent();
            this.bl = bl;
            refreshDroneList = initializeDrones;
            DroneView.DataContext= new EditDroneView(bl, initializeDrones, selectedDrone,Close);
        }

        
        //private void ChangeVisibility(Visibility visibility, params StackPanel[] stackPanels)
        //{
        //    foreach (StackPanel item in stackPanels)
        //    {
        //        item.Visibility = visibility;
        //    }
        //}

        

        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{ 
        //    e.Cancel = true;
        //}
    }
}


