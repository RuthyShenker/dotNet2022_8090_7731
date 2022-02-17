
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
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

    }
}
