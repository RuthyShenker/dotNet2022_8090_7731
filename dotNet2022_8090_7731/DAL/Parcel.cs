using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        public struct Parcel
        {
            public string ParcelId { get; set; }
            public string SenderId { get; set; }
            public string GetterId { get; set; }
            public WeightCategories Weight { get; set; }
            public UrgencyStatuses Status { get; set; }
            public string DroneId { get; set; }
            public DateTime MakingParcel { get; set; }
            public DateTime PickingUp { get; set; }
            public DateTime Arrival { get; set; }
            public DateTime MatchingParcel { get; set; }
            public Parcel(string parcelId, string senderId, string getterId, WeightCategories weight, UrgencyStatuses status, string droneId, DateTime makingParcel, DateTime pickingUp, DateTime arrival, DateTime matchingParcel)
            {
                ParcelId = parcelId;
                SenderId = senderId;
                GetterId = getterId;
                Weight = weight;
                Status = status;
                DroneId = droneId;
                MakingParcel = makingParcel;
                PickingUp = pickingUp;
                Arrival = arrival;
                MatchingParcel=matchingParcel;
            }
            public override string ToString()
            {
                return $"ParcelId: {ParcelId} SenderId: {SenderId} GetterId:" +
                    $" {GetterId} Weight: {Weight} Status: {Status} DroneId: {DroneId}" +
                    $" MakingParcel: {MakingParcel} PickingUp:{PickingUp} Arrival:{Arrival}" +
                    $" MatchingParcel:{MatchingParcel} ";
            }
        }
    }
}
