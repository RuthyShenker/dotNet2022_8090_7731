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
    /// <summary>
    /// A  public class EditDroneViewModel impliments INotify
    /// </summary>
    public class EditDroneViewModel : INotify
    {
        readonly BlApi.IBL bl;
        private EditDrone drone;

        public RelayCommand<object> ChargeDroneCommand { get; set; }
        public RelayCommand<object> AssignParcelToDroneCommand { get; set; }
        public RelayCommand<object> UpdateModelOfDroneCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> DeleteDroneCommand { get; set; }
        public RelayCommand<object> OpenParcelWindowCommand { get; set; }
        public RelayCommand<object> StartOrStopSimulatorCommand { get; set; }

        /// <summary>
        /// A constructor of EditDroneViewModel that gets bl,BO.Drone.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        public EditDroneViewModel(BlApi.IBL bl, BO.Drone drone)
        {
            Refresh.Drone += RefreshDrone;
            this.bl = bl;
            Drone = Map(drone);

            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            UpdateModelOfDroneCommand = new RelayCommand<object>(UpdateDroneModel);
            ChargeDroneCommand = new RelayCommand<object>(SendOrReleaseDroneFromCharging);
            AssignParcelToDroneCommand = new RelayCommand<object>(AssignParcelToDrone);
            OpenParcelWindowCommand = new RelayCommand<object>(OpenParcelWindowC, param => Drone.Status == PO.DroneStatus.Delivery);
            DeleteDroneCommand = new RelayCommand<object>(DeleteDrone);
            StartOrStopSimulatorCommand = new RelayCommand<object>(StartOrStopSimulator);
        }

        /// <summary>
        /// A function that updates Drone View.
        /// </summary>
        private void updateDroneView(object sender, ProgressChangedEventArgs args)
        {
            Refresh.Invoke();
            Drone.Info = args.UserState;
        }

        //--worker--
        BackgroundWorker worker;
        private void updateDrone(object state) => worker.ReportProgress(0, state);
        private bool checkStop() => worker.CancellationPending;

        /// <summary>
        /// A function that Starts Or Stops Simulator.
        /// </summary>
        /// <param name="obj"></param>
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
                worker.ProgressChanged += (sender, args) => updateDroneView(sender, args);

                Refresh.workers.Add(Drone.Id, worker);
                Refresh.workers[Drone.Id].RunWorkerAsync(Drone.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
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
            }
        }

        private void SendOrReleaseDroneFromCharging(object sender)
        {
            if (Extensions.WorkerTurnOn()) return;

            if (Drone.Status == PO.DroneStatus.Free)
                bl.SendingDroneToCharge(Drone.Id);
            else
                bl.ReleasingDrone(Drone.Id);

            Refresh.Invoke();
        }

        private void OpenParcelWindowC(object MyParcel)
        {
            var parcel = MyParcel as BO.ParcelInTransfer;
            if (parcel == null)
            {
                parcel = bl.GetDrone(Drone.Id).PInTransfer;
            }
            var blParcel = bl.GetParcel(/*Drone.ParcelInTransfer.PId/**/parcel.PId);
            new ParcelView(bl, blParcel).Show();
        }

        private void AssignParcelToDrone(object sender)
        {
            if (Extensions.WorkerTurnOn()) return;

            if (Drone.Status == PO.DroneStatus.Free)
            {
                BelongingParcel();
            }
            else if (Drone.Status == PO.DroneStatus.Delivery)
            {
                if (bl.GetParcel(Drone.ParcelInTransfer.PId).PickingUp == null)
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
            if (Extensions.WorkerTurnOn()) return;

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
            return new EditDrone()
            {
                Id = drone.Id,
                Model = drone.Model,
                Weight = (PO.WeightCategories)drone.Weight,
                BatteryStatus = drone.BatteryStatus,
                Status = (PO.DroneStatus)drone.DroneStatus,
                Location = new PO.Location()
                {
                    Latitude = drone.CurrLocation.Latitude,
                    Longitude = drone.CurrLocation.Longitude
                },
                ParcelInTransfer = Map(drone.PInTransfer)
            };
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
