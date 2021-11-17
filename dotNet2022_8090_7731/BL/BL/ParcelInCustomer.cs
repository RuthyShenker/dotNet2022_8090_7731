using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
namespace IBL.BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories weight { get; set; }
        public Priority priority { get; set; }
        public ParcelStatus pStatus { get; set; }
        //	לקוח במשלוח - המקור\היעד 
        public CustomerInParcel OnTheOtherHand { get; set; }
    }
}
