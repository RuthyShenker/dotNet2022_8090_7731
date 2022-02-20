﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{

    /// <summary>
    ///A class of CustomerToList that contains:
    ///Id
    ///Name
    ///Phone
    ///SentSupplied
    ///SentNotSupplied
    ///Got
    ///InWayToCustomer
    /// </summary>
    public class CustomerToList
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SentSupplied { get; set; }
        public int SentNotSupplied { get; set; }
        public int Got { get; set; }
        public int InWayToCustomer { get; set; }
    }
}
