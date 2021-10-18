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
        }
    }
}
