using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    // <summary>
    ///A class of Station that contains:
    ///Id
    ///Name Station
    ///Station Location
    ///Num Available Positions
    ///List_BL_ChargingDrone
    /// </summary>
    public class Station
    {
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string NameStation { get; set; }
        public Location Location { get; set; }
        public int NumAvailablePositions { get; set; }
        //  רשימת רחפנם בטעינה
        public IEnumerable<ChargingDrone> LBL_ChargingDrone { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }

    }
}