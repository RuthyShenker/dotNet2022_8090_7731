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
        /// <summary>
        /// A function that gets an object of IDal.DO.Customer
        /// and Expands it to CustomerToList object and returns this object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>returns CustomerToList object </returns>
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
        /// <summary>
        /// A function that gets an object of IDal.DO.Customer
        /// and Expands it to Customer object and returns this object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>returns Customer object </returns>
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

        /// <summary>
        /// A function that gets an object of IDal.DO.Parcel and creates
        /// a new object of ParcelInCustomerand 
        /// copies the the commmon fields from IDal.DO.Parcel
        /// to ParcelInCustomer and returns it
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="Id"></param>
        /// <returns>returns an object of ParcelInCustomer</returns>
        private ParcelInCustomer CopyCommon(IDal.DO.Parcel parcel, int Id)
        {
            var PStatus = GetParcelStatus(parcel);
            var OnTheOtherHand = NewCustomerInParcel(Id);
            return new ParcelInCustomer(parcel.Id, (IBL.BO.WeightCategories)parcel.Weight, (IBL.BO.Priority)parcel.MPriority, PStatus, OnTheOtherHand);
        }


        /// <summary>
        /// A function that 
        /// returns the list of customers that have packages delivered to them.
        /// </summary>
        /// <returns>returns the list of customers that have packages delivered to them. </returns>
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
        /// <summary>
        /// A function that gets Customer and adds it to the data base the
        /// function doesn't return anything.
        /// </summary>
        /// <param name="bLCustomer"></param>
        public void AddingCustomer(Customer bLCustomer)
        {
            if (dal.IsIdExistInList<IDal.DO.Customer>(bLCustomer.Id))
            {
                throw new IdIsNotValidException("The id is already exists in the Customer List!");
            }
            var newCustomer = new IDal.DO.Customer(bLCustomer.Id,bLCustomer.Name,
            bLCustomer.Phone,bLCustomer.CLocation.Longitude,bLCustomer.CLocation.Latitude);
            dal.AddingCustomer(newCustomer);
        }
        /// <summary>
        /// A function that gets customerId,newName,newPhone
        ///and updates the customer with id of customerId
        ///and updates its fields of name to newName and phone to newPhone
        /// and updates it in the data base,the function doesn't return anything.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="newName"></param>
        /// <param name="newPhone"></param>
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
            catch (IDal.DO.IdNotExistInTheListException)
            {
                throw new UpdatingFailedIdNotExistsException("This Id Not Exist In Customer List");
            }
        }
    }
}
