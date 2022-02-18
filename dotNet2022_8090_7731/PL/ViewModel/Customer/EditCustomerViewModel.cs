using PO;
using PL.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class EditCustomerViewModel : INotify
    {
        readonly BlApi.IBL bl;
        //readonly Action refreshCustomers;
        private EditCustomer customer;
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> UpdateCustomerCommand { get; set; }
        public RelayCommand<object> DeleteCustomerCommand { get; set; }
        public RelayCommand<object> ShowParcelOfCustomerCommand { get; set; }

        public EditCustomerViewModel(BlApi.IBL bl, BO.Customer customer)
        {
            Refresh.Customer += RefreshCustomer;
            this.bl = bl;
            Customer = MapFromBOToPO(customer);
            //this.refreshCustomers = refreshCustomers;
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            UpdateCustomerCommand = new RelayCommand<object>(UpdateCustomer, param => Customer.Error == "");
            DeleteCustomerCommand = new RelayCommand<object>(DeleteCustomer);
            ShowParcelOfCustomerCommand = new RelayCommand<object>(MouseDoubleClick);
        }

        private void MouseDoubleClick(object obj)
        {
            var parcel = obj as BO.ParcelInCustomer;
            var blParcel = bl.GetParcel(parcel.Id);
            new ParcelView(bl, blParcel).Show();
        }

        private void DeleteCustomer(object obj)
        {
            if (Customer.LForCustomer.Any() || Customer.LFromCustomer.Any())
            {
                if (Customer.LForCustomer.Any() && Customer.LFromCustomer.Any())
                    MessageBox.Show("You Can't Delete Me!,I Have Parcels For Me And To Me! ");
                else if (Customer.LForCustomer.Any())
                    MessageBox.Show("You Can't Delete Me!" +
                 ",I Have Parcels For Me ! ");
                else if(Customer.LFromCustomer.Any())
                       MessageBox.Show("You Can't Delete Me!" +
                    ",I Have Parcels From Me ! ");
                return;
            }

            if (MessageBox.Show("Are You Sure You Want To Delete Customer" +
                 $"With Id:{Customer.Id}?", "Delete Customer", MessageBoxButton.YesNo
                 , MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                MessageBox.Show(bl.DeleteCustomer(Customer.Id));
                Refresh.Invoke();

                Functions.CloseWindow(obj);
            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void UpdateCustomer(object obj)
        {
            try
            {
                bl.UpdatingCustomerDetails(Customer.Id, Customer.Name, Customer.Phone);
                Refresh.Invoke();
                //refreshCustomers();
            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RefreshCustomer()
        {
            if (bl.GetCustomers().FirstOrDefault(c => c.Id == Customer.Id) != default)
            {
                Customer = MapFromBOToPO(bl.GetCustomer(Customer.Id));
            }

        }

        private EditCustomer MapFromBOToPO(BO.Customer customer)
        {
         
            return new EditCustomer(customer.Id, customer.Name, customer.Phone,
                customer.Location.Longitude, customer.Location.Latitude,
               Map( customer.LFromCustomer), Map(customer.LFromCustomer));
        }

        private IEnumerable<ParcelInCustomer> Map(IEnumerable<BO.ParcelInCustomer> lFromCustomer)
        {
            return lFromCustomer.Select(p => convert(p));
        }

        private ParcelInCustomer convert(BO.ParcelInCustomer p)
        {
            return new()
            {
                Id = p.Id,
                MPriority = (PO.Priority)p.MPriority,
                OnTheOtherHand = new() { Id =p.OnTheOtherHand.Id, Name = p.OnTheOtherHand.Name },
                PStatus =(PO.ParcelStatus)p.PStatus,
                Weight =(PO.WeightCategories)p.Weight
            };
        }

        public EditCustomer Customer
        {
            get => customer;
            private set
            {
                customer = value;
                RaisePropertyChanged(nameof(Customer));
            }
        }

    }
}