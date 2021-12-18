using IBL.BO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PL.Drones
{
    /// <summary>
    /// Interaction logic for EditDroneView.xaml
    /// </summary>
    public partial class EditDroneView : UserControl
    {
        private IBL.IBL bl;
        Action refreshDroneList;

        public EditDroneView(IBL.IBL bl, Action initializeDrones, Drone selectedDrone)
        {
            InitializeComponent();
            this.bl = bl;
            refreshDroneList = initializeDrones;
            EditDronePanel.DataContext = selectedDrone;
        }

        private void Update_Model_Click(object sender, RoutedEventArgs e)
        {
            Drone drone = EditDronePanel.DataContext as Drone;

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
                RefreshDrone(drone);
            }
        }

        private void Send_Or_Release_Drone_From_Charging(object sender, RoutedEventArgs e)
        {
            Drone drone = EditDronePanel.DataContext as Drone;

            if (drone.DroneStatus == DroneStatus.Free)
                bl.SendingDroneToCharge(drone.Id);
            else
                bl.ReleasingDrone(drone.Id);
            RefreshDrone(drone);
        }
        private void Send_Or_pick_Or_Arrival_Drone_Click(object sender, RoutedEventArgs e)
        {
            Drone drone = EditDronePanel.DataContext as Drone;
            if (drone.DroneStatus == DroneStatus.Free)
            {
                BelongingParcel(drone);
            }
            else if (drone.DroneStatus == DroneStatus.Delivery && !drone.PInTransfer.IsInWay && !drone.PInTransfer.Equals(default(ParcelInTransfer)))
            {
                PickingUpParcel(drone);
            }
            else if (drone.DroneStatus == DroneStatus.Delivery && drone.PInTransfer.IsInWay && !drone.PInTransfer.Equals(default(ParcelInTransfer)))
            {
                DeliveryPackage(drone);
            }
        }

        private void DeliveryPackage(Drone drone)
        {
            try
            {
                bl.DeliveryPackage(drone.Id);
                RefreshDrone(drone);
            }
            catch (IBL.BO.InValidActionException exception)
            {
                MessageBox.Show(exception.Message, "Error Delivery Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PickingUpParcel(Drone drone)
        {
            try
            {
                bl.PickingUpParcel(drone.Id);
                RefreshDrone(drone);
            }
            catch (IBL.BO.InValidActionException exception)
            {
                MessageBox.Show(exception.Message, "Error Pick Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BelongingParcel(Drone drone)
        {
            //if( drone.PInTransfer.Equals(default(ParcelInTransfer)))
            try
            {
                bl.BelongingParcel(drone.Id);
                RefreshDrone(drone);
            }
            catch (IBL.BO.ThereIsNoMatchObjectInListException exception)
            {
                MessageBox.Show(exception.ExceptionDetails, "Error Belong Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (IBL.BO.ListIsEmptyException ex)
            {
                MessageBox.Show(ex.Message, "Error Belong Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshDrone(Drone drone)
        {
            refreshDroneList();
            EditDronePanel.DataContext = bl.GetDrone(drone.Id);
        }
    }
}
