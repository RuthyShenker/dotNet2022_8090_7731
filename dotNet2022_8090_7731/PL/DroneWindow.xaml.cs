﻿using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            DroneDetails.DataContext = new DroneToList();
            DroneDetails.Visibility = Visibility.Visible;
        }
        public DroneWindow(IBL.IBL bl, Action initializeDrones, DroneToList selectedDrone)
        {
            this.bl = bl;
            refreshDroneList = initializeDrones;

            InitializeComponent();

            DroneDetails.DataContext = selectedDrone;
            EnableOfTextbox();
            //GridOfAddDrone.Visibility = Visibility.Collapsed;
            //GridOfUpdateDrone.Visibility = Visibility.Visible;
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
            //DetailsDroneGrid.FindName
            if (!string.IsNullOrWhiteSpace(IdTextBox.Text) || !string.IsNullOrWhiteSpace(ModelTextBox.Text))
            {
                var a = (Button)sender;

                ToolTip toolTip = new ToolTip();
                toolTip.Content = "field is not full";
                toolTip.IsOpen = true;
                a.ToolTip = toolTip;
                return;
            }

            CheckValidDrone((DroneToList)DroneDetails.DataContext, (Button)sender);
        }



        private void CheckValidDrone(DroneToList drone, Button sender)
        {
            //            Id
            //WeightCategoriesEnum
            //DroneStatusEnum
            //DeliveredParcelId


            MessageBox.Show($"{drone.Model},{drone.CurrLocation.Longitude } {drone.CurrLocation}, {drone.CurrLocation.Latitude}");
            bool isExist = bl.GetDrones().Any(d => d.Id == drone.Id);
            if (isExist)
            {
                var textBox = (TextBox)DroneDetails.FindName("IdTextBox");
                AddToolTip(textBox, " Id is not available ");
                return;
            }

            //if (drone.CurrLocation.Longitude is < (-90) or > 90)
            //{
            //    var textBox = (TextBox)DetailsDroneGrid.FindName("Longitude");
            //    AddToolTip(textBox, "InCorrect longitude (-90, 90) ");
            //    return;
            //}

            //if (drone.CurrLocation.Latitude is < (-90) or > 90)
            //{
            //    var textBox = (TextBox)DetailsDroneGrid.FindName("Latitude");
            //    AddToolTip(textBox, "InCorrect Latitude (-90, 90) ");
            //    return;
            //}



            // to finish
        }

        void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (e.Handled = new Regex("[^0-9]+").IsMatch(e.Text))
            {
                textBox.Background = Brushes.Gray;
                ToolTip toolTip = new ToolTip();
                AddToolTip(textBox, " Input has to contain only digits ");
            }
            else
            {
                textBox.Background = Brushes.White;
            }
        }

        private void AddToolTip(TextBox textBox, string str)
        {

            ToolTip toolTip = new ToolTip();
            toolTip.Content = str;
            toolTip.IsOpen = true;
            textBox.ToolTip = toolTip;

            // Turn off ToolTip
            DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1), IsEnabled = true };
            timer.Tick += new EventHandler(delegate (object timerSender, EventArgs timerArgs)
            {
                toolTip.IsOpen = false;
                timer = null;
            });
        }

        private void Close_Drone_Window_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update_Model_Click(object sender, RoutedEventArgs e)
        {
            DroneToList drone = DroneDetails.DataContext as DroneToList;

            // when we changed bl.GetDrones to return new list 
            // before it changed ldronetolist and in the dal ?why??????????

            if (MessageBox.Show($"Are You Sure You Want To Change The Model Of The Drone With Id:{drone.Id} ?",
                   "Update Model",
                   MessageBoxButton.OKCancel,
                   MessageBoxImage.Question) == MessageBoxResult.OK)
            {
               
                bl.UpdatingDroneName(drone.Id, drone.Model);
                MessageBox.Show($"Drone With Id:{drone.Id} Updated successfuly!",
                   $" Model Updated Successly {MessageBoxImage.Information}");
                refreshDroneList();
                DroneDetails.DataContext = bl.GetDrone(drone.Id);
            }
        }

        private void Send_Or_Release_Drone_From_Charging(object sender, RoutedEventArgs e)
        {
            DroneToList drone = DroneDetails.DataContext as DroneToList;

            if (drone.DStatus == DroneStatus.Free)
                bl.SendingDroneToCharge(drone.Id);
            else
                bl.ReleasingDrone(drone.Id);
            refreshDroneList();
        }

        private void Send_Or_pick_Or_Arrival_Drone_Click(object sender, RoutedEventArgs e)
        {
            DroneToList drone = DroneDetails.DataContext as DroneToList;
            if (drone.DStatus == DroneStatus.Free)
            {
                bl.PickingUpParcel(drone.Id);
            }

        }

    }
}


