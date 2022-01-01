using BO;
using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    public class AddParcelViewModel
    {
        public ParcelToAdd Parcel { get; set; }
        BlApi.IBL bl;
        Action refreshParcels;

        public RelayCommand<object> AddParcelCommand { get; set; }

        public AddParcelViewModel(BlApi.IBL bl, Action refreshParcels)
        {
            Parcel = new();
            this.bl = bl;
            this.refreshParcels = refreshParcels;
            AddParcelCommand = new RelayCommand<object>(AddParcel);
        }

        private void AddParcel(object obj)
        {
            ////try
            ////{
            ////    bl.AddingParcel(Map(Parcel));
            ////}
            ////catch ()
            ////{

            ////}
        }

        private Parcel Map(ParcelToAdd parcel)
        {
            return new Parcel(parcel.Id, parcel.Sender, parcel.Getter, parcel.Weight, parcel.MPriority, parcel.DInParcel, parcel.MakingParcel,
                parcel.BelongParcel, parcel.PickingUp,parcel.Arrival);
        }
    }
}

