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
    public class CustomerListViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        public IEnumerable<CustomerToList> customerList ;
        public RelayCommand<object> AddCustomerCommand { get; set; }
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public CustomerListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            customerList = Enumerable.Empty<CustomerToList>();
            RefreshCustomerList();
            AddCustomerCommand = new RelayCommand<object>(AddingCustomer);
            MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);  
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MouseDoubleClick(object sender)
        {
            var selectedCustomer = sender as CustomerToList;
            var blCustomer = bl.GetCustomer(selectedCustomer.Id);
            new CustomerView(bl, RefreshCustomerList, blCustomer)
                .Show();
        }
        private void AddingCustomer(object sender)
        {
            //if (bl.AvailableSlots().Select(slot => slot.Id).Count() > 0)
            //{
            //    //var viewModel = new AddDroneViewModel(bl,FilterDroneListByCondition);
            //    new DroneView(bl, FilterDroneListByCondition);
            //    //new DroneView(/*bl,*/FilterDroneListByCondition).Show();
            //}
            //else
            //{
            //    MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            new CustomerView(bl,RefreshCustomerList)
                .Show();
        }
        private void RefreshCustomerList()
        {
            CustomerList = bl.GetCustomers();
        }
        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

    }
}
