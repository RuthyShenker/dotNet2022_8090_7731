using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL.BO
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
        public int PId { get; set; }
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
