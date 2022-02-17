
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    /// <summary>
    /// A public class EditParcel
    /// </summary>
    public class EditParcel
    {
        /// <summary>
        /// An empty constructor.
        /// </summary>
        public EditParcel()
        {

        }

        /// <summary>
        /// A constructor of EditParcel with part of params.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="getterId"></param>
        /// <param name="weight"></param>
        /// <param name="mPriority"></param>
        /// <param name="dInParcel"></param>
        public EditParcel(int senderId, int getterId, WeightCategories weight, Priority mPriority, DroneInParcel dInParcel)
           : this(0, new CustomerInParcel(senderId, string.Empty), new CustomerInParcel(getterId, string.Empty),
                weight, mPriority, dInParcel, DateTime.Now, null, null, null)
        {
        }

        /// <summary>
        /// A constructor of Parcel with all the fields.
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
        public EditParcel(int id, CustomerInParcel sender, CustomerInParcel getter, WeightCategories parcelWeight,
            Priority parcelMPriority, DroneInParcel dInParcel, DateTime parcelMakingParcel,
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
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Getter { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public DroneInParcel DInParcel { get; set; }
        public DateTime MakingParcel { get; set; }
        public DateTime? BelongParcel { get; set; }
        public DateTime? PickingUp { get; set; }
        public DateTime? Arrival { get; set; }

    }
}
