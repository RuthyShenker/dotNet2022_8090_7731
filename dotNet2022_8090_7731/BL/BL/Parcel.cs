using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {

        public Parcel(int senderId, int getterId, WeightCategories weight, Priority mPriority, DroneInParcel dInParcel)
        {
            Sender.Id = senderId;
            Getter.Id = getterId;
            ///WHATS ABOUT THEIR NAME
            Weight = weight;
            MPriority = mPriority;
            DInParcel = dInParcel;
            MakingParcel = new DateTime.Now();
            // לעשות default באמת?
            BelongParcel = default(DateTime);
            PickingUp =default(DateTime);
            Arrival = default(DateTime);
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
