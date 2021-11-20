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
        //מה הולך פה עם הbl  וdl
        public IEnumerable<CustomerToList> GetCustomers()
        {
            IEnumerable<CustomerToList> bLCustomersList = new List<CustomerToList>();
            IEnumerable<IDal.DO.Customer> dalCustomersList = dal.GetListFromDal<IDal.DO.Customer>();

            //  לשים לב לשורה הזו אם משנים לגנרי
            IEnumerable< IDal.DO.Parcel> dalParcelsList = dal.GetListFromDal<IDal.DO.Parcel>();

            foreach (var customer in dalCustomersList)
            {
                bLCustomersList.Add(MapToList(customer, dalParcelsList));
            }
            return bLCustomersList.;
        }

        private CustomerToList MapToList(IDal.DO.Customer customer, IEnumerable<Parcel> dParcels)
        {
            CustomerToList CustomerToList = new CustomerToList();
            CustomerToList.Id = customer.Id;
            CustomerToList.Name = customer.Name;
            CustomerToList.Phone = customer.Phone;
            foreach (var parcel in dParcels)
            {
                if (parcel.SenderId == CustomerToList.Id)
                {
                    switch (GetParcelStatus(parcel))
                    {
                        // לסדר אין לי תרגום ל supplied
                        case ParcelStatus.InDestination:
                            ++CustomerToList.Got;
                            break;
                        case ParcelStatus.collected:
                            ++CustomerToList.SentSupplied;
                            break;
                        case ParcelStatus.belonged:
                            ++CustomerToList.InWayToCustomer;
                            break;
                        case ParcelStatus.made:
                            ++CustomerToList.SentNotSupplied;
                            break;
                    }
                }
            }
            return CustomerToList;
        }
    }
}
