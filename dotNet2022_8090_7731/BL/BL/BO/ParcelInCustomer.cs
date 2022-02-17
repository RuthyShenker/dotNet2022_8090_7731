using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    ///A class of ParcelInCustomer that contains:
    ///Id
    ///Weight
    ///MPriority
    ///ParcelStatus
    ///OnTheOtherHand
    /// </summary>
    public class ParcelInCustomer
    { 

        /// <summary>
        /// this fiels is innit.
        /// </summary>
        public int Id { get; init; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public ParcelStatus PStatus { get; set; }
        
        /// <summary>
        /// customer in delivery -sender/getter.
        /// </summary>
        public CustomerInParcel OnTheOtherHand { get; set; }

        /// <summary>
        /// A function that returns the details of this Parcel In Customer
        /// </summary>
        /// <returns> the details of this Parcel In Customer</returns>
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}

