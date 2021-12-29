using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
namespace PL.ViewModels
{
    public class ParcelListViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        public IEnumerable<ParcelToList> parcelList;

        public ParcelListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            parcelList= Enumerable.Empty<ParcelToList>();

        }

        public IEnumerable<ParcelToList> ParcelList
        {
            get => parcelList;
            set
            {
                parcelList = value;
                RaisePropertyChanged(nameof(ParcelList));
            }
        }   

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
