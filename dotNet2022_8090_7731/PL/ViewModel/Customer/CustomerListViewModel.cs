using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static PL.Extensions;
using PL.View;
using PO;

namespace PL.ViewModels
{
    /// <summary>
    ///   public class CustomerListViewModel : INotify
    /// </summary>
    public class CustomerListViewModel : INotify
    {
        readonly BlApi.IBL bl;

        public IEnumerable<CustomerToList> customerList { get; set; }
        public RelayCommand<object> AddCustomerCommand { get; set; }
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        /// <summary>
        /// A constructor of CustomerListViewModel that gets bl.
        /// </summary>
        /// <param name="bl"></param>
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

        /// <summary>
        /// A function that opens specific customer.
        /// </summary>
        /// <param name="sender"></param>
        private void MouseDoubleClick(object sender)
        {
            if (sender == null) return;

            var selectedCustomer = sender as CustomerToList;
            try
            {
                var blCustomer = bl.GetCustomer(selectedCustomer.Id);
                new CustomerView(bl, blCustomer)
                    .Show();
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                ShowXMLExceptionMessage(exception.Message);
            }
        }

        /// <summary>
        /// A function that adds new customer.
        /// </summary>
        /// <param name="sender"></param>
        private void AddingCustomer(object sender)
        {
            new CustomerView(bl).Show();
        }

        /// <summary>
        /// A function that refreshs Customers List.
        /// </summary>
        private void RefreshCustomersList()
        {
            CustomerList = new ObservableCollection<CustomerToList>(bl.GetCustomers().MapListFromBLToPL());
        }
    }
}
