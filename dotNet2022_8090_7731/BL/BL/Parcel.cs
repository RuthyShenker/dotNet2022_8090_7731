using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    ///A class of Parcel that contains:
    ///Id
    ///Sender
    /// Getter
    ///Weight
    ///MPriority
    ///DInParcel
    ///MakingParcel
    ///BelongParcel
    ///PickingUp
    ///Arrival
    /// </summary>
    public class Parcel
    {

        public Parcel(int senderId, int getterId, WeightCategories weight, Priority mPriority, DroneInParcel dInParcel)
            : this(0, new CustomerInParcel(senderId, string.Empty), new CustomerInParcel(getterId, string.Empty),
                 weight, mPriority, dInParcel, DateTime.Now, null, null, null)
        {
        }

        public Parcel(int id, CustomerInParcel sender, CustomerInParcel getter,WeightCategories parcelWeight,
            Priority parcelMPriority,DroneInParcel dInParcel,DateTime parcelMakingParcel,
               DateTime? parcelBelongParcel, DateTime? parcelPickingUp, DateTime? parcelArrival)
        {
            Id = id;
            Sender = sender;
            Getter = getter;
            Arrival = parcelArrival;
            PickingUp = parcelPickingUp;
            BelongParcel = parcelBelongParcel;
            Weight = parcelWeight;
            MPriority = parcelMPriority;
            DInParcel = dInParcel;
            MakingParcel = parcelMakingParcel;
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
