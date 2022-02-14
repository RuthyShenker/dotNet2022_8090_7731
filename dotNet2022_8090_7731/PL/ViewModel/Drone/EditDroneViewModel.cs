﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;
using PL.View;
using PO;
namespace PL.ViewModels
{
    public class EditDroneViewModel : INotify
    {
        BlApi.IBL bl;
        Action refreshDrones;
        EditDrone drone;

        public EditDroneViewModel(BlApi.IBL bl, BO.Drone drone)
        {
            Refresh.Drone += RefreshDrone;
            this.bl = bl;
            Drone = Map(drone);
            //this.refreshDrones = refreshDrones;
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            UpdateModelOfDroneCommand = new RelayCommand<object>(UpdateDroneModel);
            ChargeDroneCommand = new RelayCommand<object>(SendOrReleaseDroneFromCharging);
            AssignParcelToDroneCommand = new RelayCommand<object>(AssignParcelToDrone);
            OpenParcelWindowCommand = new RelayCommand<object>(OpenParcelWindowC, param => Drone.Status == DroneStatus.Delivery);
            DeleteDroneCommand = new RelayCommand<object>(DeleteDrone);
        }

        
        public RelayCommand<object> ChargeDroneCommand { get; set; }

        public RelayCommand<object> AssignParcelToDroneCommand { get; set; }

        public RelayCommand<object> UpdateModelOfDroneCommand { get; set; }

        public RelayCommand<object> CloseWindowCommand { get; set; }

        public RelayCommand<object> DeleteDroneCommand { get; set; }
        public RelayCommand<object> OpenParcelWindowCommand { get; set; }
        
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

                Refresh.Invoke();

                //RefreshDrone();
            }
        }
        private void SendOrReleaseDroneFromCharging(object sender)
        {
            if (Drone.Status == DroneStatus.Free)
                bl.SendingDroneToCharge(Drone.Id);
            else
                bl.ReleasingDrone(Drone.Id);

            Refresh.Invoke();
           
        }
       
        private void OpenParcelWindowC(object MyParcel)
        {
            var parcel = MyParcel as BO.ParcelInTransfer;
            var blParcel = bl.GetParcel(parcel.PId);
            new ParcelView(bl,blParcel).Show();
        }

        private void AssignParcelToDrone(object sender)
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
                Refresh.Invoke();

                //RefreshDrone();
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
                Refresh.Invoke();
                //RefreshDrone();
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
                Refresh.Invoke();
                //RefreshDrone();
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
        private void DeleteDrone(object obj)
        {
            if(Drone.Status==DroneStatus.Delivery)
            {
                MessageBox.Show("You Can't Delete Me!" +
                  ",I Am On Way To Brong Parcel  ! ","Delete Parcel",MessageBoxButton.OK,MessageBoxImage.Stop);
                return;
            }
            else if (Drone.Status == DroneStatus.Maintenance)
            {
                MessageBox.Show("You Can't Delete Me!" +
                  ",I Am On Charging ! ", "Delete Parcel", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            //צריך לבדוק אם צריך עוד מקרים

            if (MessageBox.Show("Are You Sure You Want To Delete Drone" +
                $"With Id:{Drone.Id}?", "Delete Drone", MessageBoxButton.YesNo
                , MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                MessageBox.Show(bl.DeleteDrone(Drone.Id));
                Refresh.Invoke();

                Functions.CloseWindow(obj);
            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


        private void RefreshDrone()
        {
            Drone = Map(bl.GetDrone(Drone.Id));
        }

        private EditDrone Map(Drone drone)
        {
            return new EditDrone(drone.Id, drone.Model, drone.Weight, drone.BatteryStatus,
                drone.DroneStatus, new PO.Location(drone.CurrLocation.Latitude, drone.CurrLocation.Longitude)
                , drone.PInTransfer);
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
    }
}
