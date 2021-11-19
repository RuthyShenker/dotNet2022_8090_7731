using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {

        public Parcel(int senderId, int getterId, WeightCategories weight, Priority mPriority, DroneInParcel dInParcel, DateTime makingParcel, DateTime? belongParcel, DateTime? pickingUp, DateTime? arrival)
        {
            SenderId = senderId;
            GetterId = getterId;
            Weight = weight;
            MPriority = mPriority;
            DInParcel = dInParcel;
            MakingParcel = makingParcel;
            BelongParcel = belongParcel;
            PickingUp = pickingUp;
            Arrival = arrival;
        }

        public int ParcelId { get; init; }
        public int SenderId { get; set; }
        public int GetterId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public DroneInParcel DInParcel { get; set; }
        public DateTime MakingParcel { get; set; }
        public DateTime? BelongParcel { get; set; }
        public DateTime? PickingUp { get; set; }
        public DateTime? Arrival { get; set; }
    }
}
