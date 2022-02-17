using PL.View;
using PO;
using System;
using System.Linq;
using System.Windows;

namespace PL.ViewModels
{
    public class EditParcelViewModel : INotify
    {
        readonly BlApi.IBL bl;
        //readonly Action refreshParcels;
        private EditParcel parcel;

        public RelayCommand<object> DeleteParcelCommand { get; set; }
        public RelayCommand<object> UpdateParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> EditCustomerCommand { get; set; }
        public RelayCommand<object> OpenDroneWindowCommand { get; set; }
        public RelayCommand<object> CollectAndDeliverPackageCommand { get; set; }

        public EditParcelViewModel(BlApi.IBL bl, BO.Parcel parcel)
        {
            this.bl = bl;
            Refresh.Parcel += RefreshParcel;
            Parcel = Map(parcel);
            //this.refreshParcels = refreshParcels;
            UpdateParcelCommand = new RelayCommand<object>(UpdateParcel, param => Parcel.BelongParcel == default);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            EditCustomerCommand = new RelayCommand<object>(EditSender);
            DeleteParcelCommand = new RelayCommand<object>(DeleteParcel);
            CollectAndDeliverPackageCommand = new RelayCommand<object>(GivingPermissionToCollectAndDeliverPackage);
            OpenDroneWindowCommand = new RelayCommand<object>(OpenDroneWindow);
        }

        private void GivingPermissionToCollectAndDeliverPackage(object obj)
        {
            if (Parcel.BelongParcel == null)
            {
                MessageBox.Show("In Order to Pick up parcel, you need to belong it to drone ", "Error Pick Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Parcel.PickingUp == null)
            {
                try
                {
                    bl.PickingUpParcel(Parcel.DInParcel.Id);
                    Refresh.Invoke();

                }
                catch (BO.InValidActionException exception)
                {
                    MessageBox.Show(exception.Message, "Error Pick Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (Parcel.Arrival == null)
            {
                try
                {
                    bl.DeliveryPackage(Parcel.DInParcel.Id);
                    Refresh.Invoke();

                    //RefreshDrone();
                }
                catch (BO.InValidActionException exception)
                {
                    MessageBox.Show(exception.Message, "Error Delivery Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The Parcel Had Already arrived", "Error Delivery Parcel To drone", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteParcel(object obj)
        {

            if (Parcel.BelongParcel.HasValue)
            {
                MessageBox.Show("You can't delete me! I am belonging to a drone", "Delete Parcel Error", MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }


            if (MessageBox.Show("Are You Sure You Want To Delete Parcel" +
                $"With Id:{Parcel.Id}?", "Delete Parcel", MessageBoxButton.YesNo
                , MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                MessageBox.Show(bl.DeleteParcel(Parcel.Id));
                Refresh.Invoke();
                //refreshCustomers();
                Functions.CloseWindow(obj);

            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }

        }
        private void OpenDroneWindow(object obj)
        {
            if (Parcel.BelongParcel != null&& !Parcel.Arrival.HasValue)
            {
                var carringDrone = bl.GetDrone(Parcel.DInParcel.Id);
                new DroneView(bl, carringDrone).Show();
            }
            else
            {
                MessageBox.Show("You can't see more because the parcel doesnt meet the conditions", "Error Open Drone",MessageBoxButton.OK,MessageBoxImage.Stop);
            }
           
        }

        private void EditSender(object customerId)
        {
            if (Parcel.BelongParcel != default && Parcel.Arrival == default)
            {
                BO.Customer blCustomer = bl.GetCustomer((int)customerId);
                new CustomerView(bl, blCustomer).Show();
            }
            else
            {
                MessageBox.Show("You can't see more because the parcel doesnt meet the conditions", "Error Open Customer", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void UpdateParcel(object obj)
        {

            //TODO
        }

        private EditParcel Map(BO.Parcel parcel)
        {
            return new EditParcel
            {
                Id = parcel.Id,
                Sender = new CustomerInParcel(parcel.Sender.Id, parcel.Sender.Name),
                Getter = new CustomerInParcel(parcel.Getter.Id, parcel.Getter.Name),
                Arrival = parcel.Arrival,
                PickingUp = parcel.PickingUp,
                BelongParcel = parcel.BelongParcel,
                Weight = parcel.Weight,
                MPriority = parcel.MPriority,
                DInParcel = parcel.DInParcel,
                MakingParcel = parcel.MakingParcel
            };

            //parcel.Id,
            //    new CustomerInParcel( parcel.Sender.Id, parcel.Sender.Name),
            //    new CustomerInParcel(parcel.Getter.Id, parcel.Getter.Name),
            //    parcel.Weight, parcel.MPriority, parcel.DInParcel,
            //    parcel.MakingParcel, parcel.BelongParcel, parcel.PickingUp, parcel.Arrival);
        }
        public EditParcel Parcel
        {
            get => parcel;
            private set
            {
                parcel = value;
                RaisePropertyChanged(nameof(Parcel));
            }
        }

        private void RefreshParcel()
        {
            //refreshParcels();

            if (bl.GetParcels().FirstOrDefault(p => p.Id == Parcel.Id) != default)
            {
                Parcel = Map(bl.GetParcel(Parcel.Id));
            }
        }
    }
}
