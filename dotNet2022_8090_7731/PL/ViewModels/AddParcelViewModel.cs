using BO;
using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PL.ViewModels
{
    public class AddParcelViewModel
    {
        public ParcelToAdd Parcel { get; set; }
        BlApi.IBL bl;
        Action refreshParcels;
        public List<int> IdOption { get; set; }

        public RelayCommand<object> AddParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public AddParcelViewModel(BlApi.IBL bl, Action refreshParcels)
        {
            Parcel = new();
            this.bl = bl;
            this.refreshParcels = refreshParcels;
            AddParcelCommand = new RelayCommand<object>(AddParcel);
            IdOption = bl.GetCustomers().Select(customer => customer.Id).ToList();
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
        }
        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }
        private void AddParcel(object obj)
        {
            try
            {
                var parcel = Map(Parcel);
                bl.AddingParcel(parcel);
                MessageBox.Show("The Parcel Added Succeesfully!");
            }
            catch (IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }



        }

        private Parcel Map(ParcelToAdd parcel)
        {
            return new Parcel(parcel.Sender.Id, parcel.Getter.Id,
                parcel.Weight, parcel.MPriority, null);
        }
    }
}

