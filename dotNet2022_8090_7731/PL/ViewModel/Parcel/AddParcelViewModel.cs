
using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static PL.Extensions;
namespace PL.ViewModels
{
    public class AddParcelViewModel
    {
        public ParcelToAdd Parcel { get; set; }

        readonly BlApi.IBL bl;
       // readonly Action refreshParcels;
        public List<int> IdOption { get; set; }

        readonly Action<BO.Parcel> switchView;

        public RelayCommand<object> AddParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public AddParcelViewModel(BlApi.IBL bl, Action<BO.Parcel> switchView)
        {
            this.switchView = switchView;
            Parcel = new();
            this.bl = bl;
            AddParcelCommand = new RelayCommand<object>(AddParcel, param => Parcel.Error == string.Empty);
            IdOption = bl.GetCustomers().Select(customer => customer.Id).ToList();
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
        }
       
        private void AddParcel(object obj)
        {
            try
            {
                var parcel = Map(Parcel);
                bl.AddingParcel(parcel);
                Refresh.Invoke();
                MessageBox.Show("The Parcel Added Succeesfully!");
                switchView(parcel);
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException)
            {

                MessageBox.Show();

            }
        }

        private BO.Parcel Map(ParcelToAdd parcel)
        {
            return new BO.Parcel(){

               Id= 0,
               Sender= new() {  Id=parcel.Sender.Id,Name=string.Empty },
               Getter= new() { Id = parcel.Getter.Id, Name = string.Empty},
                Weight=(BO.WeightCategories)parcel.Weight,
                MPriority= (BO.Priority)parcel.MPriority,
                DInParcel =null, 
                 MakingParcel=DateTime.Now,
                BelongParcel= null, 
               PickingUp= null,
                 Arrival=null
            };
        }
    }
}

