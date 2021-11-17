using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
namespace IBL.BO
{
    class Parcel
    {
        public int ParcelId { get; init; }
        public string SenderId { get; set; }
        public string GetterId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MPriority { get; set; }
        public DroneInParcel DInParcel { get; set; }
        public DateTime MakingParcel { get; set; }
        public DateTime? BelongParcel { get; set; }
        public DateTime? PickingUp { get; set; }
        public DateTime? Arrival { get; set; }
    }
}
