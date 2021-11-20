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
        public void UpdatingCustomerDetails(string customerId, string newName, string newPhone)
        {
            if (!dal.ExistsInCustomerList(customerId))
            {
                throw new Exception("this id doesnt exist in customer list!");
            }
            IDal.DO.Customer customer = dal.GetCustomer(customerId);
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
        public void AddingCustomer(Customer bLCustomer)
        {
            if (dal.ExistsInCustomerList(bLCustomer.Id))
            {
                throw new Exception("The id is already exists in the Customer List!");
            }
            IDal.DO.Customer newCustomer = new IDal.DO.Customer() { Id = bLCustomer.Id, 
                Name = bLCustomer.Name, Phone = bLCustomer.Phone, Latitude = bLCustomer.CLocation.Latitude,
                Longitude = bLCustomer.CLocation.Longitude };
            dal.AddingCustomer(newCustomer);
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
