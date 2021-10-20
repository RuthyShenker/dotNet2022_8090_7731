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

            //זמן יצירת חבילה למשלוח
            public DateTime MakingParcel { get; set; }

            //זמן שיוך חבילה לרחפן
            public DateTime BelongParcel { get; set; }

            //זמן איסוף חבילה מהשולח
            public DateTime PickingUp { get; set; }

            //זמן הגעת החבילה למקבל
            public DateTime Arrival { get; set; }
           
            public Parcel(string parcelId, string senderId, string getterId, WeightCategories weight, UrgencyStatuses status, string droneId, DateTime makingParcel,DateTime belongParcel,DateTime pickingUp,DateTime arrival)
            {
                ParcelId = parcelId;
                SenderId = senderId;
                GetterId = getterId;
                Weight = weight;
                Status = status;
                DroneId = droneId;
                MakingParcel = makingParcel; 
                BelongParcel= BelongParcel;
                PickingUp = pickingUp;
                Arrival = arrival;

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
