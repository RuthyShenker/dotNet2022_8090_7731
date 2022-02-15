using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }

    }
}

