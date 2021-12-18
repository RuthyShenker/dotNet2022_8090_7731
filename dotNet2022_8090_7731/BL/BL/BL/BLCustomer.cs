﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBL.BO;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// A function that gets an object of IDAL.DO.Parcel and creates
        /// a new object of ParcelInCustomerand 
        /// copies the the commmon fields from IDAL.DO.Parcel
        /// to ParcelInCustomer and returns it
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="Id"></param>
        /// <returns>returns an object of ParcelInCustomer</returns>
        private ParcelInCustomer CopyCommon(IDAL.DO.Parcel parcel, int Id)
        {
            var PStatus = GetParcelStatus(parcel);
            var OnTheOtherHand = NewCustomerInParcel(Id);
            return new ParcelInCustomer(parcel.Id, (WeightCategories)parcel.Weight, (Priority)parcel.MPriority, PStatus, OnTheOtherHand);
        }

        /// <summary>
        /// A function that gets id of customer and builds from it an object
        /// of CustomerInParcel and Of course considering logic and returns 
        /// the new object of CustomerInParcel.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>eturns 
        /// the new object of CustomerInParcel</returns>
        private CustomerInParcel NewCustomerInParcel(int Id)
        {
            try
            {
                string name = dal.GetFromDalById<IDAL.DO.Customer>(Id).Name;
                return new CustomerInParcel(Id, name);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(IDAL.DO.Customer), Id);
            }
        }
        /// <summary>
        /// A function that 
        /// returns the list of customers that have packages delivered to them.
        /// </summary>
        /// <returns>returns the list of customers that have packages delivered to them. </returns>
        private IList<Customer> CustomersWithProvidedParcels()
        {
            IDAL.DO.Customer customer;
            var wantedCustomersList = new List<Customer>();
            var customersDalList = dal.GetListFromDal<IDAL.DO.Customer>();
            var parcelsDalList = dal.GetListFromDal<IDAL.DO.Parcel>();
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
        public int Add(Customer bLCustomer)
        {
            if (dal.IsIdExistInList<IDAL.DO.Customer>(bLCustomer.Id))
            {
                throw new IdIsAlreadyExistException(typeof(IDAL.DO.Customer), bLCustomer.Id);
            }

            var newCustomer = new IDAL.DO.Customer(bLCustomer.Id, bLCustomer.Name,
            bLCustomer.Phone, bLCustomer.Location.Longitude, bLCustomer.Location.Latitude);
            dal.Add(newCustomer);
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
        public void UpdatingCustomerDetails(int customerId, string newName, string newPhone)
        {
            try
            {
                IDAL.DO.Customer customer = dal.GetFromDalById<IDAL.DO.Customer>(customerId);

                if (!string.IsNullOrEmpty(newName))
                {
                    dal.Update<IDAL.DO.Customer>(customerId, newName, nameof(customer.Name));
                }
                if (!string.IsNullOrEmpty(newPhone))
                {
                    dal.Update<IDAL.DO.Customer>(customerId, newPhone, nameof(customer.Phone));
                }
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(IDAL.DO.Customer), customerId);
            }
        }

        public IEnumerable<CustomerToList> GetCustomers()
        {
            return dal.GetListFromDal<IDAL.DO.Customer>()
                .Select(s => ConvertToList(s));
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Customer
        /// and Expands it to CustomerToList object and returns this object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>returns CustomerToList object </returns>
        private CustomerToList ConvertToList(IDAL.DO.Customer customer)
        {
            CustomerToList nCustomer = new(customer.Id, customer.Name, customer.Phone);
            var dParcelslist = dal.GetListFromDal<IDAL.DO.Parcel>();
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

        public Customer GetCustomer(int customerId)
        {
            try
            {
                var dCustomer = dal.GetFromDalById<IDAL.DO.Customer>(customerId);
                return ConvertToBL(dCustomer);
            }
            catch (DalObject.IdIsNotExistException)
            {
                throw new IdIsNotExistException(typeof(Customer), customerId);
            }
        }

        /// <summary>
        /// A function that gets an object of IDAL.DO.Customer
        /// and Expands it to Customer object and returns this object.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>returns Customer object </returns>
        private Customer ConvertToBL(IDAL.DO.Customer customer)
        {
            var nLocation = new Location(customer.Longitude, customer.Latitude);
            Customer nCustomer = new Customer(customer.Id, customer.Name, customer.Phone, nLocation);
            ParcelInCustomer parcelInCustomer;
            var dParcelslist = dal.GetListFromDal<IDAL.DO.Parcel>();
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
    }
}

