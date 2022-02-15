using BO;
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
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        // two lists
        public IEnumerable<ParcelInCustomer> LFromCustomer { get; set; }
        public IEnumerable<ParcelInCustomer> LForCustomer { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }

    }
}

