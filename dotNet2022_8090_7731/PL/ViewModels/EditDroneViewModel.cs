using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;
using PO;
namespace PL
{
    public class EditDroneViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        Action refreshDrones;
        EditDrone drone;

        public EditDroneViewModel(BlApi.IBL bl, BO.Drone drone, Action refreshDrones)
        {
            this.bl = bl;
            Drone = Map(drone);
            this.refreshDrones = refreshDrones;
            CloseWindowCommand = new RelayCommand<object>(Close_Window);
            UpdateModelOfDroneCommand = new RelayCommand<object>(UpdateDroneModel);
            ChargeDroneCommand = new RelayCommand<object>(SendOrReleaseDroneFromCharging);
            AssignParcelToDroneCommand = new RelayCommand<object>(SendOrPickOrArrivalDrone);
        }

        public RelayCommand<object> ChargeDroneCommand { get; set; }

        public RelayCommand<object> AssignParcelToDroneCommand { get; set; }

        public RelayCommand<object> UpdateModelOfDroneCommand { get; set; }

        public RelayCommand<object> CloseWindowCommand { get; set; }

        private void UpdateDroneModel(object sender)
        {
            // when we changed bl.GetDrones to return new list 
            // before it changed ldronetolist and in the dal ?why??????????

            if (MessageBox.Show($"Are You Sure You Want To Change The Model Of The Drone With Id:{Drone.Id} ?",
                   "Update Model",
                   MessageBoxButton.OKCancel,
                   MessageBoxImage.Question) == MessageBoxResult.OK)
            {

                bl.UpdatingDroneName(Drone.Id, Drone.Model);
                MessageBox.Show($"Drone With Id:{Drone.Id} Updated successfuly!",
                   $" Model Updated Successly {MessageBoxImage.Information}");
                RefreshDrone();
            }
        }
        private void SendOrReleaseDroneFromCharging(object sender)
        {
            if (Drone.Status == DroneStatus.Free)
                bl.SendingDroneToCharge(Drone.Id);
            else
                bl.ReleasingDrone(Drone.Id);
            RefreshDrone();
        }
        private void SendOrPickOrArrivalDrone(object sender)
        {
            if (Drone.Status == DroneStatus.Free)
            {
                BelongingParcel();
            }
            else if (Drone.Status == DroneStatus.Delivery)
            {
                if (Drone.ParcelInTransfer.IsInWay)
                    DeliveryPackage();
                else
                    PickingUpParcel();
            }
        }

        private void DeliveryPackage()
        {
            try
            {
                bl.DeliveryPackage(Drone.Id);
                RefreshDrone();
            }
            catch (InValidActionException exception)
            {
                MessageBox.Show(exception.Message, "Error Delivery Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PickingUpParcel()
        {
            try
            {
                bl.PickingUpParcel(Drone.Id);
                RefreshDrone();
            }
            catch (InValidActionException exception)
            {
                MessageBox.Show(exception.Message, "Error Pick Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BelongingParcel()
        {
            //if( drone.PInTransfer.Equals(default(ParcelInTransfer)))
            try
            {
                bl.BelongingParcel(Drone.Id);
                RefreshDrone();
            }
            catch (ThereIsNoMatchObjectInListException exception)
            {
                MessageBox.Show(exception.ExceptionDetails, "Error Belong Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (ListIsEmptyException ex)
            {
                MessageBox.Show(ex.Message, "Error Belong Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshDrone()
        {
            refreshDrones();
            Drone = Map(bl.GetDrone(Drone.Id));
        }

        private EditDrone Map(Drone drone)
        {
            return new EditDrone(drone.Id, drone.Model, drone.Weight, drone.BatteryStatus,
                drone.DroneStatus, drone.CurrLocation, drone.PInTransfer);
        }

        public EditDrone Drone
        {
            get => drone;
            private set
            {
                drone = value;
                RaisePropertyChanged(nameof(Drone));
            }
        }

        private void Close_Window(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
