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
    /// <summary>
    ///  public class EditCustomerViewModel : INotify
    /// </summary>
    public class EditCustomerViewModel : INotify
    {
        readonly BlApi.IBL bl;

        private EditCustomer customer;
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public RelayCommand<object> UpdateCustomerCommand { get; set; }
        public RelayCommand<object> DeleteCustomerCommand { get; set; }
        public RelayCommand<object> ShowParcelOfCustomerCommand { get; set; }

        /// <summary>
        /// A constructor of EditCustomerViewModel that gets bl, BO.Customer.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="customer"></param>
        public EditCustomerViewModel(BlApi.IBL bl, BO.Customer customer)
        {
            Refresh.Customer += RefreshCustomer;
            this.bl = bl;
            Customer = MapFromBOToPO(customer);

            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
            UpdateCustomerCommand = new RelayCommand<object>(UpdateCustomer, param => Customer.Error == "");
            DeleteCustomerCommand = new RelayCommand<object>(DeleteCustomer, param => param != null);
            ShowParcelOfCustomerCommand = new RelayCommand<object>(MouseDoubleClick, param => param != null);
        }


        /// <summary>
        /// A function that opens parcel of customer.
        /// </summary>
        /// <param name="sender"></param>
        private void MouseDoubleClick(object obj)
        {
            if (Extensions.WorkerTurnOn()) return;

            var parcel = obj as BO.ParcelInCustomer;
            var blParcel = bl.GetParcel(parcel.Id);
            new ParcelView(bl, blParcel).Show();
        }

        /// <summary>
        /// A function that deletes specific customer.
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteCustomer(object obj)
        {
            if (Extensions.WorkerTurnOn()) return;
           
            if (Customer.LForCustomer.Any() || Customer.LFromCustomer.Any())
            {
                if (Customer.LForCustomer.Any() && Customer.LFromCustomer.Any())
                    MessageBox.Show("You Can't Delete Me!,I Have Parcels For Me And To Me! ");
                else if (Customer.LForCustomer.Any())
                    MessageBox.Show("You Can't Delete Me!" +
                 ",I Have Parcels For Me ! ");
                else if (Customer.LFromCustomer.Any())
                    MessageBox.Show("You Can't Delete Me!" +
                 ",I Have Parcels From Me ! ");
                return;
            }

            if (MessageBox.Show("Are You Sure You Want To Delete Customer" +
                 $"With Id:{Customer.Id}?", "Delete Customer", MessageBoxButton.YesNo
                 , MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                MessageBox.Show(bl.DeleteCustomer(Customer.Id));
                Refresh.Invoke();

                Functions.CloseWindow(obj);
            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// A function that Updates Customer
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateCustomer(object obj)
        {
            if (Extensions.WorkerTurnOn()) return;

            try
            {
                bl.UpdatingCustomerDetails(Customer.Id, Customer.Name, Customer.Phone);
                Refresh.Invoke();
            }
            catch (BO.IdIsNotExistException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// A function that Refreshes Customer
        /// </summary>
        private void RefreshCustomer()
        {
            if (bl.GetCustomers().FirstOrDefault(c => c.Id == Customer.Id) != default)
            {
                Customer = MapFromBOToPO(bl.GetCustomer(Customer.Id));
            }

        }

        /// <summary>
        /// A function that converts BO.Customer to EditCustomer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>EditCustomer</returns>
        private EditCustomer MapFromBOToPO(BO.Customer customer)
        {
            return new EditCustomer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Location = new Location() { Latitude = customer.Location.Latitude, Longitude = customer.Location.Longitude },
                LForCustomer = Map(customer.LForCustomer),
                LFromCustomer = Map(customer.LFromCustomer)
            };
        }

        /// <summary>
        /// A function that converts IEnumerable<BO.ParcelInCustomer> to IEnumerable<ParcelInCustomer>
        /// </summary>
        /// <param name="lFromCustomer"></param>
        /// <returns>IEnumerable<ParcelInCustomer></returns>
        private IEnumerable<ParcelInCustomer> Map(IEnumerable<BO.ParcelInCustomer> lFromCustomer)
        {
            return lFromCustomer.Select(p => convert(p));
        }

        /// <summary>
        /// a function that converts BO.ParcelInCustomer to ParcelInCustomer
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private ParcelInCustomer convert(BO.ParcelInCustomer p)
        {
            return new()
            {
                Id = p.Id,
                MPriority = (Priority)p.MPriority,
                OnTheOtherHand = new() { Id = p.OnTheOtherHand.Id, Name = p.OnTheOtherHand.Name },
                PStatus = (ParcelStatus)p.PStatus,
                Weight = (WeightCategories)p.Weight
            };
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
    }
}