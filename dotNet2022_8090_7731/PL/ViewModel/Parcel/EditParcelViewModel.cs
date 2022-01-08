using BO;
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
    public class EditParcelViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        Action refreshParcels;
        EditParcel parcel;

        public RelayCommand<object> UpdateParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        
        public EditParcelViewModel(BlApi.IBL bl, BO.Parcel parcel, Action refreshParcels)
        {
            this.bl = bl;
            Parcel = Map(parcel);
            this.refreshParcels = refreshParcels;
            UpdateParcelCommand = new RelayCommand<object>(UpdateParcel);
            CloseWindowCommand = new RelayCommand<object>(Close_Window);
        }

        private void UpdateParcel(object obj)
        {

        }

        private EditParcel Map(Parcel parcel)
        {
            //return new EditParcel(parcel.Id, parcel.Sender, parcel.Getter, parcel.Weight, parcel.MPriority, parcel.DInParcel,
            //    parcel.MakingParcel, parcel.BelongParcel, parcel.PickingUp, parcel.Arrival);
            return new EditParcel();
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
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshParcel()
        {
            refreshParcels();
            Parcel = Map(bl.GetParcel(Parcel.Id));
        }

        private void Close_Window(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
