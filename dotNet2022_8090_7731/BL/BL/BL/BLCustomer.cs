using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    /// <summary>
    /// An internal sealed partial class BL inherits from Singleton<BL>,and impliments BlApi.IBL,
    /// </summary>
    partial class BL:BlApi.IBL
    {

        /// <summary>
        /// A function that gets Customer and adds it to the data base the
        /// function doesn't return anything.
        /// </summary>
        /// <param name="bLCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddCustomer(Customer bLCustomer)
        {
            lock (dal)
            {
                if (dal.IsIdExistInList<DO.Customer>(bLCustomer.Id))
                {
                    throw new IdAlreadyExistsException(typeof(Customer), bLCustomer.Id);
                }

                var newCustomer = new DO.Customer()
                {
                    Id = bLCustomer.Id,
                    Name = bLCustomer.Name,
                    Phone = bLCustomer.Phone,
                    Longitude = bLCustomer.Location.Longitude,
                    Latitude = bLCustomer.Location.Latitude
                };
                dal.Add(newCustomer);
            }
            return bLCustomer.Id;
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatingCustomerDetails(int customerId, string newName, string newPhone)
        {
            try
            {
                lock (dal)
                {
                    DO.Customer customer = dal.GetFromDalById<DO.Customer>(customerId);

                    if (!string.IsNullOrEmpty(newName))
                    {
                        dal.Update<DO.Customer>(customerId, newName, nameof(customer.Name));
                    }
                    if (!string.IsNullOrEmpty(newPhone))
                    {
                        dal.Update<DO.Customer>(customerId, newPhone, nameof(customer.Phone));
                    }
                }
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Customer), customerId);
            }
        }

        /// <summary>
        /// A function that returns all the customers from the db.
        /// </summary>
        /// <returns>returns all the customers from the db</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetCustomers()
        {
            //lock (dal) 
            //{
            return dal.GetListFromDal<DO.Customer>()
                   .Select(c => ConvertToList(c));
            //}
        }

        /// <summary>
        /// A function that gets id of customer and searchs this customer in the db and returns it after converts to bl type.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>returns customer after convers it to bl type.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            try
            {
                DO.Customer dCustomer;
                lock (dal)
                {
                    dCustomer = dal.GetFromDalById<DO.Customer>(customerId);
                }
                return ConvertToBL(dCustomer);
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Customer), customerId);
            }
        }
        /// <summary>
        /// A function that gets id of customer and deletes the instance with this id from the db,
        /// returns string that the action Performed successfully.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>returns string that the action Performed successfully</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string DeleteCustomer(int customerId)
        {
            try
            {
                lock (dal)
                {
                    dal.Remove(dal.GetFromDalById<DO.Customer>(customerId));
                }
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException(typeof(Customer), customerId);
            }
            return $"The Customer with Id: {customerId} was successfully removed from the system";
        }

        /// <summary>
        /// A function that gets an insatnce of IDAL.DO.Parcel and creates
        /// a new insatnce of ParcelInCustomerand 
        /// copies the the commmon fields from IDAL.DO.Parcel
        /// to ParcelInCustomer and returns it
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="Id"></param>
        /// <returns>returns an insatnce of ParcelInCustomer</returns>
        private ParcelInCustomer CopyCommon(DO.Parcel parcel, int Id)
        {
            var PStatus = GetParcelStatus(parcel);
            var OnTheOtherHand = NewCustomerInParcel(Id);

            return new ParcelInCustomer()
            {
                Id = parcel.Id,
                Weight = (WeightCategories)parcel.Weight,
                MPriority = (Priority)parcel.MPriority,
                PStatus = PStatus,
                OnTheOtherHand = OnTheOtherHand
            };
        }

        /// <summary>
        /// A function that gets id of customer and builds from it an object
        /// of CustomerInParcel and Of course considering logic and returns 
        /// the new insatnce of CustomerInParcel.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>eturns 
        /// the new object of CustomerInParcel</returns>
        private CustomerInParcel NewCustomerInParcel(int Id)
        {
            string name;
            try
            {
                lock (dal)
                {
                    name = dal.GetFromDalById<DO.Customer>(Id).Name;
                }
                return new()
                {
                    Id = Id,
                    Name = name
                };
            }
            catch (DO.IdDoesNotExistException)
            {
                throw new IdIsNotExistException();
                //throw new IdDoesNotExistException(typeof(Customer), Id);
            }
            catch (ArgumentNullException )
            {
                throw;
            }
        }

        /// <summary>
        /// A function that 
        /// returns the list of customers that have packages delivered to them.
        /// </summary>
        /// <returns> returns the list of customers that have packages delivered to them. </returns>
        private IEnumerable<Customer> CustomersWithProvidedParcels()
        {
            DO.Customer customer;
            var reqiredCustomersList = Enumerable.Empty<Customer>();
            lock (dal)
            {
                var customersDalList = dal.GetListFromDal<DO.Customer>();
                var parcelsDalList = dal.GetListFromDal<DO.Parcel>();
                foreach (var parcel in parcelsDalList)
                {
                    if (parcel.Arrival.HasValue)
                    {
                        customer = customersDalList.First(customer => customer.Id == parcel.GetterId);
                        reqiredCustomersList = reqiredCustomersList.Append(ConvertToBL(customer));
                    }
                }
            }
            return reqiredCustomersList;
        }

        /// <summary>
        /// A function that gets an instance of IDAL.DO.Customer
        /// and converts it to CustomerToList instance and returns this instance.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>returns CustomerToList instance </returns>
        private CustomerToList ConvertToList(DO.Customer customer)
        {
            CustomerToList nCustomer = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone
            };
            lock (dal)
            {
                #region
                //var dParcelslist = dal.GetListFromDal<DO.Parcel>();
                //foreach (var parcel in dParcelslist)
                //{
                //    if (parcel.SenderId == nCustomer.Id)
                //    {
                //        switch (GetParcelStatus(parcel))
                //        {
                //            case ParcelStatus.InDestination:
                //                ++nCustomer.SentSupplied;
                //                break;
                //            default:
                //                ++nCustomer.SentNotSupplied;
                //                break;
                //        }
                //    }
                //    else if (parcel.GetterId == nCustomer.Id)
                //    {
                //        switch (GetParcelStatus(parcel))
                //        {
                //            case ParcelStatus.InDestination:
                //                ++nCustomer.Got;
                //                break;
                //            default:
                //                ++nCustomer.InWayToCustomer;
                //                break;
                //        }
                //    }
                //}
                #endregion
                //??????????????????????????????problem????
                nCustomer.SentNotSupplied = dal.GetDalListByCondition<DO.Parcel>(p => p.SenderId == customer.Id && p.Arrival != null).Count();
                nCustomer.SentSupplied = dal.GetDalListByCondition<DO.Parcel>(p => p.SenderId == customer.Id && p.BelongParcel != null).Count();
                nCustomer.Got = dal.GetDalListByCondition<DO.Parcel>(p => p.GetterId == customer.Id && p.Arrival != null).Count();
                nCustomer.InWayToCustomer = dal.GetDalListByCondition<DO.Parcel>(p => p.GetterId == customer.Id && p.PickingUp != null).Count();

            }
            return nCustomer;
        }

        /// <summary>
        /// A function that gets an instance of IDAL.DO.Customer
        /// and converts it to Customer instance and returns this instance.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>returns Customer instance </returns>
        private Customer ConvertToBL(DO.Customer customer)
        {
            Customer nCustomer = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Location = new Location()
                {
                    Longitude = customer.Longitude,
                    Latitude = customer.Latitude
                }
            };

            ParcelInCustomer parcelInCustomer;
            var lFromCustomer = new List<ParcelInCustomer>();
            var lForCustomer = new List<ParcelInCustomer>();

            lock (dal)
            {
                var dParcelslist = dal.GetListFromDal<DO.Parcel>();
                foreach (var parcel in dParcelslist)
                {
                    if (parcel.SenderId == nCustomer.Id)
                    {
                        parcelInCustomer = CopyCommon(parcel, parcel.GetterId);
                        lFromCustomer.Add(parcelInCustomer);
                    }
                    else if (parcel.GetterId == nCustomer.Id)
                    {
                        parcelInCustomer = CopyCommon(parcel, parcel.SenderId);
                        lForCustomer.Add(parcelInCustomer);
                    }
                }
            }
            nCustomer.LFromCustomer = lFromCustomer;
            nCustomer.LForCustomer = lForCustomer;
            return nCustomer;
        }
    }
}

