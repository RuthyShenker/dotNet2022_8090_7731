using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    partial class BL
    {
        public IEnumerable<CustomerToList> GetCustomers()
        {
            IEnumerable<CustomerToList> bCustomersList = new List<CustomerToList>();
            List<IDal.DO.Customer> dCustomersList = GetList<IDal.DO.Customer>();

            //  לשים לב לשורה הזו אם משנים לגנרי
            List<Parcel> dParcelsList = GetList<Parcel>();

            foreach (var customer in dCustomersList)
            {
                bCustomersList.Add(MapToList(customer, dParcelsList));
            }
            return bCustomersList;
        }

        private CustomerToList MapToList(IDal.DO.Customer customer, List<Parcel> dParcels)
        {
            CustomerToList nCustomer = new CustomerToList();
            nCustomer.Id = customer.Id;
            nCustomer.Name = customer.Name;
            nCustomer.Phone = customer.Phone;
            foreach (var parcel in dParcels)
            {
                if (parcel.SenderId == nCustomer.Id)
                {
                    switch (GetParcelStatus(parcel))
                    {
                        // לסדר אין לי תרגום ל supplied
                        case InDestination:
                            ++nCustomer.Got;
                        case collected:
                            ++nCustomer.SentSupplied;
                        case belonged:
                            ++nCustomer.InWayToCustomer;
                        case made:
                            ++nCustomer.SentNotSupplied;
                    }
                }
            }
            return nCustomer;
        }
    }
}
