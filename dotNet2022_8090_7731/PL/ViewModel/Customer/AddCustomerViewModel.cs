using PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static PL.Extensions;
namespace PL.ViewModels
{
    /// <summary>
    /// public class AddCustomerViewModel
    /// </summary>
    public class AddCustomerViewModel
    {
        public List<string> PhoneOptions { get; set; }
        public string Prefix { get; set; }
        readonly BlApi.IBL bl;
        readonly Action<BO.Customer> switchView;
        public CustomerToAdd Customer { get; set; }
        public RelayCommand<object> AddCustomerCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        /// <summary>
        /// constructor of AddCustomerViewModel gets bl,Action
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="switchView"></param>
        public AddCustomerViewModel(BlApi.IBL bl,  Action<BO.Customer> switchView)
        {
            Customer = new();
            this.bl = bl;
            this.switchView = switchView;
            AddCustomerCommand = new RelayCommand<object>(AddCustomer, param => Customer.Error == "");
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            PhoneOptions = new List<string>() {"052","050","054","058","053" ,"055","056"};
        }

        /// <summary>
        /// A function that adds customer.
        /// </summary>
        /// <param name="obj"></param>
        private void AddCustomer(object obj)
        {
            try
            {
                var blCustomer = MapCustomerFromPOToBO(Customer);
                bl.AddCustomer(blCustomer);
                Refresh.Invoke();
                MessageBox.Show("The Customer Added Succeesfully!");
                switchView(blCustomer);
            }
            catch (BO.IdAlreadyExistsException exception)
            {
               ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException ex)
            {
                MessageBox.Show( $"fail to load xml file: {ex.xmlFilePath}", "Error  to load xml file ");
            }
            catch (BO.ListIsEmptyException exception)
            {
                ShowIdExceptionMessage(exception.Message);
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
