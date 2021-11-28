﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// A class of Customer that contains:
    /// Id
    ///Name
    ///Phone
    ///CustomerLocation
    ///ListFromCustomer
    ///ListForCustomer
    /// </summary>
    public class Customer
    {
        public Customer(int id, string name, string phone, Location cLocation)
        {
            Id = id;
            Name = name;
            Phone = phone;
            CLocation = cLocation;
            LFromCustomer = new List<ParcelInCustomer>();
            LForCustomer =new List<ParcelInCustomer>();
        }
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location CLocation { get; set; }
        // שתי רשימות
        public List<ParcelInCustomer> LFromCustomer { get; set; }
        public List<ParcelInCustomer> LForCustomer { get; set; }
    }
}
