using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
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
        public ParcelInCustomer(int id, WeightCategories weight, Priority mPriority, ParcelStatus pStatus, CustomerInParcel onTheOtherHand)
        {
            Id = id;
            Weight = weight;
            MPriority = mPriority;
            PStatus = pStatus;
            OnTheOtherHand = onTheOtherHand;
        }

        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public ParcelStatus PStatus { get; set; }
        //	לקוח במשלוח - המקור\היעד 
        public CustomerInParcel OnTheOtherHand { get; set; }
    }
}
