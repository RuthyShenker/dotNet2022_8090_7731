﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject
    {

        /// <summary>
        /// A function that gets a Customer and adds it to the list of Customers.
        /// </summary>
        /// <param name="customer"></param>
        public void AddingCustomer(Customer customer)
        {
            CustomerList.Add(customer);
        }

        /// <summary>
        /// A function that gets an id of customer and returns this customer-copied.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer CustomerDisplay(string Id)
        {
            for (int i = 0; i < CustomerList.Count; i++)
            {
                if (CustomerList[i].Id == Id)
                {
                    return CustomerList[i].Clone();
                }
            }
            throw new Exception("id doesnt exist");
            //return CustomerList.First(customer => customer.Id == Id).Clone();

        }


        /// <summary>
        /// A function that returns the list of the customers
        /// </summary>
        /// <returns> customer list</returns>
        public IEnumerable<Customer> DisplayingCustomers()
        {
            return new List<Customer>(CustomerList.Select(customer => new Customer(customer)).ToList());
        }


    }
}
