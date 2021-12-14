using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
        /// <summary>
        /// A constructor of Customer with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="location"></param>
        public Customer(int id, string name, string phone, Location location)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Location = location;
            LFromCustomer = new List<ParcelInCustomer>();
            LForCustomer =new List<ParcelInCustomer>();
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        // two lists
        public List<ParcelInCustomer> LFromCustomer { get; set; }
        public List<ParcelInCustomer> LForCustomer { get; set; }
    }
}
