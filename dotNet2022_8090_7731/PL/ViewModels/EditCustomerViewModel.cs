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
        EditCustomer customer;
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
            //new ParcelView(bl, refreshParcelList, blParcel).Show();
        }

        private void DeleteCustomer(object obj)
        {
            try
            {
                var customer = obj as BO.Customer;
                MessageBox.Show(bl.DeleteCustomer(customer.Id));
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
                var customer = obj as BO.Customer;
                bl.UpdatingCustomerDetails(customer.Id,customer.Name,customer.Phone);
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
            return new EditCustomer(customer.Id,customer.Name,customer.Phone,new PO.Location(customer.Location),
                customer.LFromCustomer,customer.LForCustomer);
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