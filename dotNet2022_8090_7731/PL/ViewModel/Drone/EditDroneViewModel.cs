using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static PL.Extensions;
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
        bool isCharging;

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
            isCharging = Drone.Status == DroneStatus.Maintenance;

            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            UpdateModelOfDroneCommand = new RelayCommand<object>(UpdateDroneModel);
            ChargeDroneCommand = new RelayCommand<object>(SendOrReleaseDroneFromCharging);
            AssignParcelToDroneCommand = new RelayCommand<object>(AssignParcelToDrone);
            OpenParcelWindowCommand = new RelayCommand<object>(OpenParcelWindow, param => Drone.Status == PO.DroneStatus.Delivery);
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
                    if (isCharging)
                    {
                        MessageBox.Show("Can't turn on simulator when drone is charging, please release it", "Drone in charging", MessageBoxButton.OK);
                        return;
                    }

                    Refresh.workers[Drone.Id].RunWorkerAsync(Drone.Id);
                }
                else
                {
                    Refresh.workers[Drone.Id]?.CancelAsync();
                }
            }
            else
            {
                if (isCharging)
                {
                    MessageBox.Show("Can't turn on simulator when drone is charging, please release it", "Drone in charging", MessageBoxButton.OK);
                    return;
                }

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
        /// A function that Updates Drone Model
        /// </summary>
        /// <param name="sender"></param>
        private void UpdateDroneModel(object sender)
        {

            if (Extensions.WorkerTurnOn()) return;


            if (MessageBox.Show($"Are You Sure You Want To Change The Model Of The Drone With Id:{Drone.Id} ?",
                   "Update Model",
                   MessageBoxButton.OKCancel,
                   MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                try
                {
                    bl.UpdatingDroneName(Drone.Id, Drone.Model);
                }
                catch (BO.IdDoesNotExistException exception)
                {
                    ShowIdExceptionMessage(exception.Message);
                }
                catch (BO.XMLFileLoadCreateException)
                {
                    MessageBox.Show();
                }
                MessageBox.Show($"Drone With Id:{Drone.Id} Updated successfuly!",
                   $" Model Updated Successly {MessageBoxImage.Information}");

                Refresh.Invoke();
            }
        }
        /// <summary>
        /// A function that Sends Or Releases Drone From Charging
        /// </summary>
        /// <param name="sender"></param>
        private void SendOrReleaseDroneFromCharging(object sender)
        {
            if (Extensions.WorkerTurnOn()) return;
            try
            {
                if (Drone.Status == PO.DroneStatus.Free)
                    bl.SendingDroneToCharge(Drone.Id);
                else
                    bl.ReleasingDrone(Drone.Id);
            }
            catch (BO.InValidActionException exception)
            {
                ShowTheExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }

            if (Drone.Status == DroneStatus.Free)
            {
                bl.SendingDroneToCharge(Drone.Id);
                isCharging = true;
            }
            else
            {
                bl.ReleasingDrone(Drone.Id);
                isCharging = false;
            }

            Refresh.Invoke();
        }

        /// <summary>
        /// A function that opens parcel.
        /// </summary>
        /// <param name="MyParcel"></param>
        private void OpenParcelWindow(object MyParcel)
        {
            var parcel = MyParcel as BO.ParcelInTransfer;
            if (parcel == null)
            {
                parcel = bl.GetDrone(Drone.Id).PInTransfer;
            }
            var blParcel = bl.GetParcel(parcel.PId);
            new ParcelView(bl, blParcel).Show();
        }


        /// <summary>
        /// A function that belongs parcel to drone.
        /// </summary>
        /// <param name="sender"></param>
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

        /// <summary>
        /// A function that Delivery Package to destination.
        /// </summary>
        private void DeliveryPackage()
        {
            try
            {
                bl.DeliveryPackage(Drone.Id);
                Refresh.Invoke();
            }
            catch (BO.InValidActionException exception)
            {
                ShowTheExceptionMessage(exception.Message, "Error Delivery Parcel To drone");
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }
        }

        /// <summary>
        /// A function that Picking Up Parcel by drone.
        /// </summary>
        private void PickingUpParcel()
        {
            try
            {
                bl.PickingUpParcel(Drone.Id);
                Refresh.Invoke();
            }
            catch (BO.InValidActionException exception)
            {
                ShowTheExceptionMessage(exception.Message, "Error Pick Parcel To drone");
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }
        }

        /// <summary>
        /// A function that Belonging Parcel to drone.
        /// </summary>
        private void BelongingParcel()
        {
            //if( drone.PInTransfer.Equals(default(ParcelInTransfer)))
            try
            {
                bl.BelongingParcel(Drone.Id);
                Refresh.Invoke();
            }
            catch (BO.ThereIsNoMatchObjectInListException exception)
            {
                ShowTheExceptionMessage(exception.Message, "Error Belong Parcel To drone");
            }
            catch (BO.ListIsEmptyException ex)
            {
                MessageBox.Show(ex.Message, "Error Belong Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.InValidActionException exception)
            {
                ShowTheExceptionMessage(exception.Message, "Error Belong Parcel To drone");

            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
        }

        /// <summary>
        /// A function that Deletes Drone.
        /// </summary>
        /// <param name="obj"></param>
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
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }
        }

        /// <summary>
        /// A function that Refreshes Drone.
        /// </summary>
        private void RefreshDrone()
        {
            try
            {
                if (bl.GetDrones().FirstOrDefault(d => d.Id == Drone.Id) != default)
                {
                    var keepSimulatorState = drone.Automatic;
                    Drone = Map(bl.GetDrone(Drone.Id));
                    Drone.Automatic = keepSimulatorState;
                }
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException)
            {
                MessageBox.Show();
            }
        }

        /// <summary>
        /// A function that converts from BO.Drone to EditDrone
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private EditDrone Map(BO.Drone drone)
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

        /// <summary>
        /// A function that converts from BO.ParcelInTransfer to PO.ParcelInTransfer.
        /// </summary>
        /// <param name="pInTransfer"></param>
        /// <returns></returns>
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
