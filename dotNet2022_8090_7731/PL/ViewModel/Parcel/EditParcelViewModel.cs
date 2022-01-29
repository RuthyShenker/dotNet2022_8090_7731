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
        EditParcel parcel;

        public RelayCommand<object> UpdateParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> EditCustomerCommand { get; set; }
        
        public EditParcelViewModel(BlApi.IBL bl, BO.Parcel parcel)
        {
            this.bl = bl;
            Refresh.Parcel += RefreshParcel;
            Parcel = Map(parcel);
            //this.refreshParcels = refreshParcels;
            UpdateParcelCommand = new RelayCommand<object>(UpdateParcel);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            EditCustomerCommand = new RelayCommand<object>(EditSender);
        }

        private void EditSender(object senderId)
        {
            var blCustomer = bl.GetCustomer((int)senderId);
            new CustomerView(bl, /*RefreshParcel*/ blCustomer)
                    .Show();
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
            Parcel = Map(bl.GetParcel(Parcel.Id));
        }
    }
}
