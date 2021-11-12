using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;

namespace IBL.BO
{
    class DroneToList
    {
        public int Id { get; set; }
        public int Model { get; set; }
        public WeightCategories Weight { get; set; }
        public float BatteryStatus { get; set; }
        public DroneStatus DStatus { get; set; }
        public Location CurrLocation { get; set; }
        public int NumOfParcel{ get; set; }
    }
}
