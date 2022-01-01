using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;
using PL.View;

namespace PL.ViewModels
{
    public class ParcelListViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        public IEnumerable<ParcelToList> parcelList;
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public RelayCommand<object> AddParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        
        private void RefreshParcelList()
        {

        }
        public ParcelListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            parcelList= Enumerable.Empty<ParcelToList>();
            ParcelList = bl.GetParcels();
            MouseDoubleCommand = new RelayCommand<object>(EditParcel);
            AddParcelCommand = new RelayCommand<object>(AddParcel);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
        }

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        private void AddParcel(object obj)
        {
            new ParcelView(bl, RefreshParcelList).Show();
        }

        private void EditParcel(object obj)
        {
            var parcel = obj as BO.ParcelToList;
            var blParcel = bl.GetParcel(parcel.Id);
            new ParcelView(bl, RefreshParcelList, blParcel).Show();
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
