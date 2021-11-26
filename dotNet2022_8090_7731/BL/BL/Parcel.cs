using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {
        public Parcel(int id, CustomerInParcel sender, CustomerInParcel getter, WeightCategories weight, Priority mPriority, DroneInParcel dInParcel,
            DateTime makingParcel, DateTime? belongParcel, DateTime? pickingUp, DateTime? arrival)
        {
            Id = id;
            Sender = sender;
            Getter = getter;
            Weight = weight;
            MPriority = mPriority;
            DInParcel = dInParcel;
            MakingParcel = makingParcel;
            BelongParcel = belongParcel;
            PickingUp = pickingUp;
            Arrival = arrival;
        }

        public int Id { get; init; }
        public CustomerInParcel Sender{ get; set; }
        public CustomerInParcel Getter{ get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public DroneInParcel DInParcel { get; set; }
        public DateTime MakingParcel { get; set; }
        public DateTime? BelongParcel { get; set; }
        public DateTime? PickingUp { get; set; }
        public DateTime? Arrival { get; set; }
    }
}
