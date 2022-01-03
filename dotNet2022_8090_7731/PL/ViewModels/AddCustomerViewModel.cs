using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class AddCustomerViewModel
    {
        BlApi.IBL bl;
        Action refreshCustomer;
        public CustomerToAdd Customer { get; set; }
        public RelayCommand<object> AddCustomerCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public AddCustomerViewModel(BlApi.IBL bl, Action refreshCustomerList)
        {
            Customer = new();
            this.bl = bl;
            refreshCustomer = refreshCustomerList;
            AddCustomerCommand = new RelayCommand<object>(AddCustomer, param => Customer.Error == "");
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
        }

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        private void AddCustomer(object obj)
        {
            try
            {
                var blCustomer = MapCustomerFromPOToBO(Customer);
                bl.AddCustomer(blCustomer);
                refreshCustomer();
            }
            catch (BO.IdIsAlreadyExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private BO.Customer MapCustomerFromPOToBO(CustomerToAdd customer)
        {
            return new BO.Customer((int)customer.Id, customer.Name, customer.Phone,
                new BO.Location((double)customer.Longitude, (double)customer.Latitude));
        }
    }
}
