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
        public List<string> PhoneOptions { get; set; }
        public string Cidomet { get; set; }

        readonly BlApi.IBL bl;
        readonly Action<BO.Customer> switchView;
        public CustomerToAdd Customer { get; set; }
        public RelayCommand<object> AddCustomerCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public AddCustomerViewModel(BlApi.IBL bl,  Action<BO.Customer> switchView)
        {
            Customer = new();
            this.bl = bl;
            this.switchView = switchView;
            AddCustomerCommand = new RelayCommand<object>(AddCustomer, param => Customer.Error == "");
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            PhoneOptions = new List<string>() {"052","050","054","058","053" ,"055","056"};
        }

        private void AddCustomer(object obj)
        {
            try
            {
                var blCustomer = MapCustomerFromPOToBO(Customer);
                bl.AddCustomer(blCustomer);
                Refresh.Invoke(); 
                switchView(blCustomer);
            }
            catch (BO.IdAlreadyExistsException exception)
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
            var phone = customer.Prefix + customer.Phone;
            return new()
            {
                Id = (int)customer.Id,
                Name = customer.Name,
                Phone = phone,
                Location = new() { Longitude = (double)customer.Longitude, Latitude = (double)customer.Latitude },
                LForCustomer = new List<BO.ParcelInCustomer>(),
                LFromCustomer = new List<BO.ParcelInCustomer>()
            };
        }
    }
}
