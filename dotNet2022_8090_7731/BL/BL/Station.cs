using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Station
    {
        public int Id { get; init; }
        public string NameStation { get; set; }
        public Location SLocation{ get; set; }
        public int NumAvailablePositions { get; set; }
        //  רשימת רחפנם בטעינה
        public List<ChargingDrone> lBL_ChargingDrone { get; set; }
    }
}
