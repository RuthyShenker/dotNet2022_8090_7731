using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PL.View;
using PO;

namespace PL.ViewModels
{
    public class CustomerListViewModel : INotify
    {
        readonly BlApi.IBL bl;

        public IEnumerable<CustomerToList> customerList { get; set; }
        public RelayCommand<object> AddCustomerCommand { get; set; }
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public CustomerListViewModel(BlApi.IBL bl)
        {
            Refresh.CustomersList += RefreshCustomersList;

            this.bl = bl;
            CustomerList = new ObservableCollection<CustomerToList>(bl.GetCustomers().MapListFromBLToPL());
            RefreshCustomersList();
            AddCustomerCommand = new RelayCommand<object>(AddingCustomer);
            MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);  
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
        }
        public IEnumerable<CustomerToList> CustomerList
        {
            get => customerList;
            set
            {
                customerList = value;
                RaisePropertyChanged(nameof(CustomerList));
            }
        }

        private void MouseDoubleClick(object sender)
        {
            var selectedCustomer = sender as CustomerToList;
            var blCustomer = bl.GetCustomer(selectedCustomer.Id);
            new CustomerView(bl, blCustomer)
                .Show();
        }
        private void AddingCustomer(object sender)
        {
            new CustomerView(bl).Show();
        }

        private void RefreshCustomersList()
        {
            CustomerList = new ObservableCollection<CustomerToList>(bl.GetCustomers().MapListFromBLToPL());
        }
    }
}
