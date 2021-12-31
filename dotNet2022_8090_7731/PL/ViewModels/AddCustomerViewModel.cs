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

        public AddCustomerViewModel(BlApi.IBL bl,Action refreshCustomerList)
        {
            Customer = new();
            this.bl = bl;
            this.refreshCustomer = refreshCustomerList;
            AddCustomerCommand = new RelayCommand<object>(AddCustomer);
        }

        private void AddCustomer(object obj)
        {
            //MessageBox.Show(Customer.Id); 
            //TODO:
            //check validation:
            var blCustomer = Map(Customer);
            bl.AddCustomer(blCustomer);
            var a= bl.GetCustomer(blCustomer.Id);
            refreshCustomer();
        }

        private BO.Customer Map(CustomerToAdd customer)
        {
            return new BO.Customer(customer.Id, customer.Name, customer.Phone,
                new BO.Location(customer.Location.Longitude,customer.Location.Latitude));
        }
    }
}
