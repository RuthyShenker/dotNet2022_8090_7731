using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using IBL.BO;
namespace BL
{
    partial class BL
    {
        /* // יש גנרי
         // //מה הולך פה עם הbl  וdl
         // public IEnumerable<CustomerToList> GetCustomers()
         // {
         //     IEnumerable<CustomerToList> bLCustomersList = new List<CustomerToList>();
         //     IEnumerable<IDal.DO.Customer> dalCustomersList = dal.GetListFromDal<IDal.DO.Customer>();
         //
         //     //  לשים לב לשורה הזו אם משנים לגנרי
         //     IEnumerable< IDal.DO.Parcel> dalParcelsList = dal.GetListFromDal<IDal.DO.Parcel>();
         //
         //     foreach (var customer in dalCustomersList)
         //     {
         //         bLCustomersList.Add(MapToList(customer, dalParcelsList));
         //     }
         //     return bLCustomersList.;
         // }

          // */

        /*  לבדוק אם צריך לאתחל שדות ל-0 
         SentSupplied, SentNotSupplied, Got, InWayToCustomer*/
        private CustomerToList ConvertToList(IDal.DO.Customer customer)
        {
            CustomerToList nCustomer = new CustomerToList(customer.Id, customer.Name, customer.Phone);
            var dParcelslist = dal.GetListFromDal<IDal.DO.Parcel>();
            foreach (var parcel in dParcelslist)
            {
                if (parcel.SenderId == nCustomer.Id)
                {
                    switch (GetParcelStatus(parcel))
                    {
                        case ParcelStatus.InDestination:
                            ++nCustomer.SentSupplied;
                            break;
                        default:
                            ++nCustomer.SentNotSupplied;
                            break;
                    }
                }
                else if (parcel.GetterId == nCustomer.Id)
                {
                    switch (GetParcelStatus(parcel))
                    {
                        case ParcelStatus.InDestination:
                            ++nCustomer.Got;
                            break;
                        default:
                            ++nCustomer.InWayToCustomer;
                            break;
                    }

                }
            }
            return nCustomer;
        }

        private Customer ConvertToBL(IDal.DO.Customer customer)
        {
            var nLocation = new Location(customer.Longitude, customer.Latitude);
            Customer nCustomer = new Customer(customer.Id, customer.Name, customer.Phone, nLocation);
            ParcelInCustomer parcelInCustomer;
            var dParcelslist = dal.GetListFromDal<IDal.DO.Parcel>();
            foreach (var parcel in dParcelslist)
            {
                if (parcel.SenderId == nCustomer.Id)
                {
                    parcelInCustomer = CopyCommon(parcel, parcel.GetterId);
                    nCustomer.LFromCustomer.Add(parcelInCustomer);
                }
                else if (parcel.GetterId == nCustomer.Id)
                {
                    parcelInCustomer = CopyCommon(parcel, parcel.SenderId);
                    nCustomer.LForCustomer.Add(parcelInCustomer);
                }
            }
            return nCustomer;
        }

        /// coppy the commmon feilds from parcel to parcel inCustomer and return it
        private ParcelInCustomer CopyCommon(IDal.DO.Parcel parcel, int Id)
        {
            var PStatus = GetParcelStatus(parcel);
            var OnTheOtherHand = NewCustomerInParcel(Id);
            return new ParcelInCustomer(parcel.Id, (IBL.BO.WeightCategories)parcel.Weight, parcel.MPriority, PStatus, OnTheOtherHand);
        }

        // מחזירה את רשימת הלקוחות שיש חבילות שסופקו להם
        private IList<Customer> CustomersWithProvidedParcels()
        {
            IDal.DO.Customer customer;
            var wantedCustomersList = new List<Customer>();
            var customersDalList = dal.GetListFromDal<IDal.DO.Customer>();
            var parcelsDalList = dal.GetListFromDal<IDal.DO.Parcel>();
            foreach (var parcel in parcelsDalList)
            {
                if (parcel.Arrival.HasValue)
                {
                    customer = customersDalList.First(customer => customer.Id == parcel.GetterId);
                    wantedCustomersList.Add(ConvertToBL(customer));
                }
            }
            return wantedCustomersList;
        }

        public void AddingCustomer(Customer bLCustomer)
        {
            if (dal.ExistsInCustomerList(bLCustomer.Id))
            {
                throw new IdIsNotValidException("The id is already exists in the Customer List!");
            }
            IDal.DO.Customer newCustomer = new IDal.DO.Customer()
            {
                Id = bLCustomer.Id,
                Name = bLCustomer.Name,
                Phone = bLCustomer.Phone,
                Latitude = bLCustomer.CLocation.Latitude,
                Longitude = bLCustomer.CLocation.Longitude
            };
            dal.AddingCustomer(newCustomer);
        }

        public void UpdatingCustomerDetails(int customerId, string newName, string newPhone)
        {
            try
            {
                IDal.DO.Customer customer = dal.GetFromDalById<IDal.DO.Customer>(customerId);
                if (!string.IsNullOrEmpty(newName))
                {
                    customer.Name = newName;
                }
                if (!string.IsNullOrEmpty(newPhone))
                {
                    customer.Phone = newPhone;
                }
                dal.UpdateCustomer(customerId, customer);
            }
            catch (IdNotExistInTheListException exception)
            {
                throw new UpdatingFailedIdNotExistsException("This Id Not Exist In Customer List");
            }
        }
    }
}
