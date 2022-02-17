using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BO;
using PL.View;
using PO;
namespace PL.ViewModels
{
    public class EditDroneViewModel : INotify
    {
        readonly BlApi.IBL bl;
        //readonly Action refreshDrones;
        private EditDrone drone;

        public RelayCommand<object> ChargeDroneCommand { get; set; }

        public RelayCommand<object> AssignParcelToDroneCommand { get; set; }

        public RelayCommand<object> UpdateModelOfDroneCommand { get; set; }

        public RelayCommand<object> CloseWindowCommand { get; set; }

        public RelayCommand<object> DeleteDroneCommand { get; set; }
        public RelayCommand<object> OpenParcelWindowCommand { get; set; }
        public RelayCommand<object> StartOrStopSimulatorCommand { get; set; }

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
            OpenParcelWindowCommand = new RelayCommand<object>(OpenParcelWindowC, param => Drone.Status == PO.DroneStatus.Delivery);
            DeleteDroneCommand = new RelayCommand<object>(DeleteDrone);
            StartOrStopSimulatorCommand = new RelayCommand<object>(StartOrStopSimulator);
        }

        private void updateDroneView()
        {
            Refresh.Invoke();

            // update distance
            //drone.Distance = args.ProgressPercentage;
            //DroneForList droneForList = Model.Drones.FirstOrDefault(d => d.Id == Drone.Id);
            //int index = Model.Drones.IndexOf(droneForList);
            //drone = bl.GetDrone(Drone.Id);
            //Model.Drones.Remove(droneForList);
            //Model.Drones.Insert(index, bl.GetDroneForList(Drone.Id));
            //updateFlags();
            //this.setAndNotify(PropertyChanged, nameof(Drone), out drone, drone);
        }

        //--worker--
        BackgroundWorker worker;
        private void updateDrone() => worker.ReportProgress(0);
        private bool checkStop() => worker.CancellationPending;

        // private void Manual_Click(object sender, RoutedEventArgs e) => worker?.CancelAsync();

        private void StartOrStopSimulator(object obj)
        {
            if (Refresh.workers.ContainsKey(Drone.Id))
            {
                if (!Refresh.workers[Drone.Id].IsBusy)
                {
                    Refresh.workers[Drone.Id].RunWorkerAsync(Drone.Id);
                }
                else
                {
                    Refresh.workers[Drone.Id]?.CancelAsync();
                }
            }
            else
            {
                worker = new()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true,
                };
                worker.DoWork += (sender, args) => bl.StartSimulator(Drone.Id, updateDrone, checkStop);
                worker.RunWorkerCompleted += (sender, args) => Drone.Automatic = false;
                worker.ProgressChanged += (sender, args) => updateDroneView();

                Refresh.workers.Add(Drone.Id, worker);

                Refresh.workers[Drone.Id].RunWorkerAsync(Drone.Id);

                //worker.RunWorkerAsync(Drone.Id);
            }


            //if (!Drone.Automatic)
            //{
            //    Drone.Automatic = true;




            //        .Add(Drone.Id, new BackgroundWorker());



            //    var w = Refresh.workers[drone.Id];

            //    Refresh.workers[drone.Id] = worker;

            //    worker = new()
            //    {
            //        WorkerReportsProgress = true,
            //        WorkerSupportsCancellation = true,
            //    };
            //    worker.DoWork += (sender, args) => bl.StartSimulator(Drone.Id, updateDrone, checkStop);
            //    worker.RunWorkerCompleted += (sender, args) => Drone.Automatic = false;
            //    worker.ProgressChanged += (sender, args) => updateDroneView();
            //    worker.RunWorkerAsync(Drone.Id);
            //}
            //else //Drone.Automatic = false
            //{
            //    worker?.CancelAsync();
            //}
        }

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
            if (Drone.Status == PO.DroneStatus.Free)
                bl.SendingDroneToCharge(Drone.Id);
            else
                bl.ReleasingDrone(Drone.Id);

            Refresh.Invoke();

        }

        private void OpenParcelWindowC(object MyParcel)
        {
            
            var parcel = MyParcel as BO.ParcelInTransfer;
            var blParcel = bl.GetParcel(/*Drone.ParcelInTransfer.PId/**/parcel.PId);
            new ParcelView(bl, blParcel).Show();
        }

        private void AssignParcelToDrone(object sender)
        {
            if (Drone.Status == PO.DroneStatus.Free)
            {
                BelongingParcel();
            }
            else if (Drone.Status == PO.DroneStatus.Delivery)
            {
                if (Drone.ParcelInTransfer == null) 
                    PickingUpParcel();
                else
                    DeliveryPackage();
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
            if (Drone.Status == PO.DroneStatus.Delivery)
            {
                MessageBox.Show("You Can't Delete Me!" +
                  ",I Am On Way To Brong Parcel  ! ", "Delete Parcel", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            else if (Drone.Status == PO.DroneStatus.Maintenance)
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
            if (bl.GetDrones().FirstOrDefault(d => d.Id == Drone.Id) != default)
            {
                var keepSimulatorState = drone.Automatic;
                Drone = Map(bl.GetDrone(Drone.Id));
                Drone.Automatic = keepSimulatorState;
            }
        }

        private EditDrone Map(Drone drone)
        {
            return new EditDrone(drone.Id, drone.Model, (PO.WeightCategories)drone.Weight, drone.BatteryStatus,
               (PO.DroneStatus)drone.DroneStatus, new PO.Location(drone.CurrLocation.Latitude, drone.CurrLocation.Longitude)
                ,Map(drone.PInTransfer));
        }

        private PO.ParcelInTransfer Map(BO.ParcelInTransfer pInTransfer)
        {
            if (pInTransfer == null)
            {
                return null;
            }
            return new()
            {
                PId = pInTransfer.PId,
                Sender = new() { Id = pInTransfer.Sender.Id, Name = pInTransfer.Sender.Name },
                Getter = new() { Id = pInTransfer.Getter.Id, Name = pInTransfer.Getter.Name },
                MPriority = (PO.Priority)pInTransfer.MPriority,
                IsInWay = pInTransfer.IsInWay,
                Weight = (PO.WeightCategories)pInTransfer.Weight,
                DeliveryLocation = new()
                {
                    Latitude = pInTransfer.DeliveryLocation.Latitude,
                    Longitude = pInTransfer.DeliveryLocation.Longitude
                },
                TransDistance = pInTransfer.TransDistance,
                CollectionLocation = new() { Latitude = pInTransfer.CollectionLocation.Latitude, Longitude = pInTransfer.CollectionLocation.Longitude }
            };
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
