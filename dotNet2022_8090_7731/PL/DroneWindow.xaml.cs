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
            GridOfAddDrone.Visibility = Visibility.Visible;
            GridOfUpdateDrone.Visibility = Visibility.Collapsed;
            detailsOfDrone.DataContext = new DroneToList();
        }
        public DroneWindow(IBL.IBL bl, Action initializeDrones, DroneToList selectedDrone)
        {            
            this.bl = bl;
            refreshDroneList = initializeDrones;

            InitializeComponent();
            
            detailsOfDrone.DataContext = selectedDrone;
            EnableOfTextbox();
            GridOfAddDrone.Visibility = Visibility.Collapsed;
            GridOfUpdateDrone.Visibility = Visibility.Visible;
            if (selectedDrone.DStatus == DroneStatus.Delivery)
                SendOrReleaseDroneFromCharging.Visibility = Visibility.Collapsed;
            else if (selectedDrone.DStatus == DroneStatus.Maintenance)
                SendOrReleaseDroneFromCharging.DataContext = "ReleaseDronefromCharge:";
            else SendOrReleaseDroneFromCharging.DataContext = "SendDroneToCharge";
        }


        private void EnableOfTextbox()
        {
            IdTextBox.IsEnabled = false;
            BatteryTextBox.IsEnabled = false;
            MaxWeightComboBox.IsEnabled = false;
            StatusComboBox.IsEnabled = false;
            DeliveryTextBox.IsEnabled = false;
            LatitudeTextBox.IsEnabled = false;
            LongitudeTextBox.IsEnabled = false;
        }

        private void Button_Click_Of_Adding_New_Drone(object sender, RoutedEventArgs e)
        {

            CheckValidDrone((DroneToList)detailsOfDrone.DataContext);
 
            int id;
            int.TryParse(IdTextBox.Text, out id);
            float batteryStatus;
            float.TryParse(BatteryTextBox.Text, out batteryStatus);
            string model = ModelTextBox.Text;
            double longitude;
            double latitude;
            double.TryParse(LongitudeTextBox.Text, out longitude);
            double.TryParse(LatitudeTextBox.Text, out latitude);
            WeightCategories weight = (WeightCategories)MaxWeightComboBox.SelectedIndex;
            DroneStatus droneStatus = (DroneStatus)StatusComboBox.SelectedIndex;
            Drone newDrone = new Drone(id, model, weight, batteryStatus, droneStatus, null, new Location(longitude, latitude));
            //bl.AddingDrone(newDrone, 0);  
            
            ///תחנה להטענה
            ///delivery?
        }

        private void CheckValidDrone(DroneToList drone)
        {
            bool check = bl.GetDrones().Any(d => d.Id == drone.Id);
            if (check == true)
            {
                TextBlock TextBlock = new TextBlock();
                TextBlock.Text= "Id is already exist";
                message.Children.Add(TextBlock);
            }
            if (drone.BatteryStatus<0 ||drone.BatteryStatus>100)
            {
                TextBlock TextBlock = new TextBlock();
                if(drone.BatteryStatus < 0)
                    TextBlock.Text = "Battery Status is less than zero";
                else TextBlock.Text = "Battery Status is more than hundred" ;

                message.Children.Add(TextBlock);
            }
            if (!drone.Model.All(ch=>char.IsNumber(ch)))
            {
                TextBlock TextBlock = new TextBlock();
                TextBlock.Text = "Id has to contain only numbers";
                 message.Children.Add(TextBlock);
            }

        }

        private void Close_Drone_Window_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update_Model_Click(object sender, RoutedEventArgs e)
        {
            DroneToList drone= detailsOfDrone.DataContext as DroneToList;

            // when we changed bl.GetDrones to return new list 
            // before it changed ldronetolist and in the dal ?why??????????
            //MessageBox.Show(bl.GetDrone(drone.Id).Model);
            //MessageBox.Show(bl.GetDrones().First(d=>d.Id==drone.Id).Model);
            bl.UpdatingDroneName(drone.Id, drone.Model);
            MessageBox.Show(bl.GetDrone(drone.Id).Model);
            MessageBox.Show(bl.GetDrones().First(d => d.Id == drone.Id).Model);
            refreshDroneList();
        }

        private void Send_Or_Release_Drone_From_Charging(object sender, RoutedEventArgs e)
        {
            DroneToList drone=detailsOfDrone.DataContext as DroneToList;

            if (drone.DStatus == DroneStatus.Free)
            {
                bl.SendingDroneToCharge(drone.Id);
                SendOrReleaseDroneFromCharging.DataContext = "ReleaseDronefromCharge:";
            }
            else
            {
                bl.ReleasingDrone(drone.Id);
                SendOrReleaseDroneFromCharging.DataContext = "SendDroneToCharge:";
            }
        }
    }
}


