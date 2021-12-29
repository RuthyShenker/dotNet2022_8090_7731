using PL.Model;
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

        public EditCustomerViewModel(BlApi.IBL bl, BO.Customer customer, Action refreshCustomers)
        {
            this.bl = bl;
            Customer = Map(customer);
            this.refreshCustomers = refreshCustomers;
            CloseWindowCommand = new RelayCommand<object>(Close_Window);
        }

        public RelayCommand<object> CloseWindowCommand { get; set; }
       
        //private void RefreshCustomer()
        //{
        //    refreshCustomers();
        //    Customer = Map(bl.GetCustomer(Customer.Id));
        //}

        private EditCustomer Map(BO.Customer drone)
        {
            return new EditCustomer();
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