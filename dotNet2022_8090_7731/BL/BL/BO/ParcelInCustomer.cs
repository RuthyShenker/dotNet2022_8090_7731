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
        /// A constructor with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="weight"></param>
        /// <param name="mPriority"></param>
        /// <param name="pStatus"></param>
        /// <param name="onTheOtherHand"></param>
        public ParcelInCustomer(int id, WeightCategories weight, Priority mPriority, ParcelStatus pStatus, CustomerInParcel onTheOtherHand)
        {
            Id = id;
            Weight = weight;
            MPriority = mPriority;
            PStatus = pStatus;
            OnTheOtherHand = onTheOtherHand;
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public ParcelStatus PStatus { get; set; }
        //	לקוח במשלוח - המקור\היעד 
        public CustomerInParcel OnTheOtherHand { get; set; }
    }
}

