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
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public ParcelStatus PStatus { get; set; }
        //	לקוח במשלוח - המקור\היעד 
        public CustomerInParcel OnTheOtherHand { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}

