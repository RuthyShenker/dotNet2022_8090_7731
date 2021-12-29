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
         
        }

    }
}

