using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace PO
{
    /// <summary>
    ///A class of ParcelInTransfer that contains:
    ///PId
    ///IsInWay
    ///MPriority
    ///Weight
    ///Sender
    ///Getter
    ///CollectionLocation
    ///DeliveryLocation
    ///TransDistance
    /// </summary>
    public class ParcelInTransfer
    {
        /// <summary>
        /// this fiels is init.
        /// </summary>
        public int PId { get; init; }
        public bool IsInWay { get; set; }
        public Priority MPriority { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Getter { get; set; }
        public Location CollectionLocation { get; set; }
        public Location DeliveryLocation { get; set; }
        public double TransDistance { get; set; }

    }
}

