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
    public class EditCustomerViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        Action refreshCustomers;
        EditCustomer Customer;
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> UpdateCustomerCommand { get; set; }
        public RelayCommand<object> DeleteCustomerCommand { get; set; }
        public RelayCommand<object> ShowParcelOfCustomerCommand { get; set; }

        public EditCustomerViewModel(BlApi.IBL bl, BO.Customer customer, Action refreshCustomers)
        {
            this.bl = bl;
            Customer = Map(customer);
            this.refreshCustomers = refreshCustomers;
            CloseWindowCommand = new RelayCommand<object>(Close_Window);
            UpdateCustomerCommand = new RelayCommand<object>(UpdateCustomer);
            DeleteCustomerCommand = new RelayCommand<object>(DeleteCustomer);
            ShowParcelOfCustomerCommand = new RelayCommand<object>(MouseDoubleClick);
        }

        private void MouseDoubleClick(object obj)
        {
            var parcel = obj as BO.ParcelInCustomer;
            var blParcel = bl.GetParcel(parcel.Id);
            new ParcelView(bl, refreshParcelList, blParcel).Show();
        }

        private void refreshParcelList()
        {
            
        }

        private void DeleteCustomer(object obj)
        {
            try
            {
                MessageBox.Show(bl.DeleteCustomer(Customer.Id));
                refreshCustomers();
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
                bl.UpdatingCustomerDetails(Customer.Id,Customer.Name,Customer.Phone);
                //TODO:
                //MessageBox.Show("",,,);
                refreshCustomers();
            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        //private void RefreshCustomer()
        //{
        //    refreshCustomers();
        //    Customer = Map(bl.GetCustomer(Customer.Id));
        //}

        private EditCustomer Map(BO.Customer customer)
        {
            return new EditCustomer(customer.Id,customer.Name,customer.Phone,
                customer.Location.Longitude, customer.Location.Latitude,
                customer.LFromCustomer,customer.LForCustomer);
        }

        //public EditCustomer Customer
        //{
        //    get => customer;
        //    private set
        //    {
        //        customer = value;
        //        RaisePropertyChanged(nameof(Customer));
        //    }
        //}

        private void Close_Window(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}