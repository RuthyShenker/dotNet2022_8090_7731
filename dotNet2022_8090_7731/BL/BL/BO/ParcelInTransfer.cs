using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
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
        public ParcelInTransfer()
        {
        }

        /// <summary>
        /// A constructor of ParcelInTransfer with fields.
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="isInWay"></param>
        /// <param name="mPriority"></param>
        /// <param name="weight"></param>
        /// <param name="sender"></param>
        /// <param name="getter"></param>
        /// <param name="collectionLocation"></param>
        /// <param name="deliveryLocation"></param>
        /// <param name="transDistance"></param>
        public ParcelInTransfer(int pId, bool isInWay, Priority mPriority, WeightCategories weight, CustomerInParcel sender,
            CustomerInParcel getter, Location collectionLocation, Location deliveryLocation, double transDistance)
        {
            PId = pId;
            IsInWay = isInWay;
            MPriority = mPriority;
            Weight = weight;
            Sender = sender;
            Getter = Getter;
            CollectionLocation = collectionLocation;
            DeliveryLocation = deliveryLocation;
            TransDistance = transDistance;
        }
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
