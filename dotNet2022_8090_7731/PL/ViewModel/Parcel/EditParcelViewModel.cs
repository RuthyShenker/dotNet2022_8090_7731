using PL.View;
using PO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class EditParcelViewModel : INotify
    {
        BlApi.IBL bl;
        Action refreshParcels;
        private EditParcel parcel;

        public RelayCommand<object> DeleteParcelCommand { get; set; }
        public RelayCommand<object> UpdateParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> EditCustomerCommand { get; set; }
        public RelayCommand<object> OpenDroneWindowCommand { get; set; }

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
            OpenDroneWindowCommand = new RelayCommand<object>(OpenDroneWindow, param => Parcel.BelongParcel != default && Parcel.Arrival != default);
        }
        private void DeleteParcel(object obj)
        {
            //if (Parcel.Arrival.HasValue /*&& bl.GetCustomers(customer => customer.Id == Parcel.Getter.Id || customer.Id == Parcel.Sender.Id).Any()*/) 
            //{
            //    MessageBox.Show( "You can't delete me! Setter Or Getter Exists,", "Delete Parcel Error", MessageBoxButton.OK,
            //        MessageBoxImage.Stop);
            //    return;
            //}
            /*else*/ if (Parcel.BelongParcel.HasValue && Parcel.PickingUp.HasValue)
            {
                MessageBox.Show("I am on the way!  You can't delete me!", "Delete Parcel Error", MessageBoxButton.OK,
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
            var carringDrone = bl.GetDrone(Parcel.DInParcel.Id);
            new DroneView(bl, carringDrone).Show();
        }

        private void EditSender(object customerId)
        {
            BO.Customer blCustomer = bl.GetCustomer((int)customerId);
            new CustomerView(bl, blCustomer).Show();
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

            if (bl.GetParcels().FirstOrDefault(p=>p.Id==Parcel.Id)!=default) 
            { 
                Parcel = Map(bl.GetParcel(Parcel.Id));
            }
        }
    }
}
