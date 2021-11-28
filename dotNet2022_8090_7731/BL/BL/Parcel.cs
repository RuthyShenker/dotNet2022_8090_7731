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
        /// <summary>
        ///A constructor of Parcel with 
        ///senderId
        ///getterId
        /// weight
        ///mPriority
        ///dInParcel
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="getterId"></param>
        /// <param name="weight"></param>
        /// <param name="mPriority"></param>
        /// <param name="dInParcel"></param>
        public Parcel(int senderId, int getterId, WeightCategories weight, Priority mPriority, DroneInParcel dInParcel)
            : this(0, new CustomerInParcel(senderId, string.Empty), new CustomerInParcel(getterId, string.Empty),
                 weight, mPriority, dInParcel, DateTime.Now, null, null, null)
        {
        }
        /// <summary>
        /// A constructor of Parcel with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sender"></param>
        /// <param name="getter"></param>
        /// <param name="parcelWeight"></param>
        /// <param name="parcelMPriority"></param>
        /// <param name="dInParcel"></param>
        /// <param name="parcelMakingParcel"></param>
        /// <param name="parcelBelongParcel"></param>
        /// <param name="parcelPickingUp"></param>
        /// <param name="parcelArrival"></param>
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
        /// <summary>
        /// this field is init.
        /// </summary>
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
