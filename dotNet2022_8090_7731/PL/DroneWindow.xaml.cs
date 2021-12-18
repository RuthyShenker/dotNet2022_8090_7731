﻿using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            AddingDrone.DataContext = new Drone();
            //ChooseMaxWeight.DataContext = Enum.GetValues(typeof(WeightCategories));
            StationIdComboBox.DataContext = bl.AvailableSlots().Select(slot => slot.Id);
            DroneDetails.Visibility = Visibility.Collapsed;
            //ChangeVisibility(Visibility.Collapsed, BatteryContainer, StatusContainer, Location);
          
        }
        private void ChangeVisibility(Visibility visibility, params StackPanel[] stackPanels)
        {
            foreach (StackPanel item in stackPanels)
            {
                item.Visibility = visibility;
            }
        }

        private void Button_Click_Ok_Adding_New_Drone(object sender, RoutedEventArgs e)
        {
            var stationId = (int)StationIdComboBox.SelectedValue;
            //var a = stationId.GetType().GetProperty("Id").GetValue("")
            var drone = (Drone)AddingDrone.DataContext;

            //var nDrone = new Drone(drone.Id, drone.Model, drone.Weight, DroneStatus.Maintenance);
            bool IsCorrect = CheckValidDrone(drone, (Button)sender);
            if (IsCorrect)
            {
                //var a = bl.GetStations();
                //var b = a.First().Id;
                bl.AddingDrone(drone, stationId);
                refreshDroneList();
                this.Close();
            }

        }

        public DroneWindow(IBL.IBL bl, Action initializeDrones, Drone selectedDrone)
        { 
            InitializeComponent();
            this.bl = bl;
            refreshDroneList = initializeDrones;

            DroneDetails.DataContext = selectedDrone;
            MessageBox.Show($"{selectedDrone.PInTransfer.Getter}");
            EnableOfTextbox();
            //GridOfAddDrone.Visibility = Visibility.Collapsed;
            //GridOfUpdateDrone.Visibility = Visibility.Visible;
        }

        private void EnableOfTextbox()
        {
            IdTextBox.IsEnabled = false;
            BatteryTextBox.IsEnabled = false;
            //MaxWeightComboBox.IsEnabled = false;
            StatusComboBox.IsEnabled = false;
            DeliveryComboBox.IsEnabled = false;
            LatitudeTextBox.IsEnabled = false;
            LongitudeTextBox.IsEnabled = false;
        }

        



        private bool CheckValidDrone(Drone drone, Button sender)
        {
            //            Id
            //WeightCategoriesEnum
            //DroneStatusEnum
            //DeliveredParcelId
            
            bool isExist = bl.IsDroneExist(drone.Id);
            if (isExist)
            {
                var textBox = (TextBox)DroneDetails.FindName("IdTextBox");
                AddToolTip(textBox, " Id is not available ");
                return false;
            }
            return true;
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
            Drone drone = DroneDetails.DataContext as Drone;

            if (drone.DroneStatus == DroneStatus.Free)
                bl.SendingDroneToCharge(drone.Id);
            else
                bl.ReleasingDrone(drone.Id);
            refreshDroneList(); 
            DroneDetails.DataContext = bl.GetDrone(drone.Id);
        }

        private void Send_Or_pick_Or_Arrival_Drone_Click(object sender, RoutedEventArgs e)
        {
            Drone drone = DroneDetails.DataContext as Drone;
            ParcelInTransfer d1 = default(ParcelInTransfer);
            if (drone.DroneStatus == DroneStatus.Free )
            {
                //if( drone.PInTransfer.Equals(default(ParcelInTransfer)))
                try
                {
                    bl.BelongingParcel(drone.Id);
                }
                catch (BL.ThereIsNoMatchObjectInList exception)
                {
                    MessageBox.Show(exception.ToString(), "Error Belong Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if(drone.DroneStatus==DroneStatus.Delivery && !drone.PInTransfer.IsInWay && !drone.PInTransfer.Equals(default(ParcelInTransfer)))
            {
                bl.PickingUpParcel(drone.Id);
            }
            else if(drone.DroneStatus==DroneStatus.Delivery && drone.PInTransfer.IsInWay && !drone.PInTransfer.Equals(default(ParcelInTransfer)))
            {
                bl.DeliveryPackage(drone.Id);
            }
            DroneDetails.DataContext = bl.GetDrone(drone.Id);
        }

    }
}


