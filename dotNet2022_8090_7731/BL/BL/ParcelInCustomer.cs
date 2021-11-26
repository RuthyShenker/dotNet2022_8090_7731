using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
{
    public class ParcelInCustomer
    {
        private WeightCategories weight1;
        private IDal.DO.UrgencyStatuses mPriority;

        public ParcelInCustomer(int id, WeightCategories weight, IDal.DO.UrgencyStatuses mPriority, ParcelStatus pStatus, CustomerInParcel onTheOtherHand)
        {
            Id = id;
            Weight = weight;
            MPriority = (Priority)mPriority;
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
