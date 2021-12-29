using BO;
using PO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    public class EditParcelViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        Action refreshParcels;
        EditParcel parcel;

        public event PropertyChangedEventHandler PropertyChanged;

        public EditParcelViewModel(BlApi.IBL bl, BO.Parcel parcel, Action refreshParcels)
        {
            this.bl = bl;
            Parcel = Map(parcel);
            this.refreshParcels = refreshParcels;
            //CloseWindowCommand = new RelayCommand<object>(Close_Window);
            //UpdateModelOfDroneCommand = new RelayCommand<object>(UpdateDroneModel);
            //ChargeDroneCommand = new RelayCommand<object>(SendOrReleaseDroneFromCharging);
            //AssignParcelToDroneCommand = new RelayCommand<object>(SendOrPickOrArrivalDrone);
        }

        private EditParcel Map(Parcel parcel)
        {
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
    }
}
