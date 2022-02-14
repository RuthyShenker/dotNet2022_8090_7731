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
        private Location sLocation;

        public Station(int id, string nameStation, Location sLocation, int numAvailablePositions)
        {
            Id = id;
            NameStation = nameStation;
            Location = sLocation;
            NumAvailablePositions = numAvailablePositions;
            LBL_ChargingDrone = null;
        }

        /// <summary>
        /// A constructor of Station with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameStation"></param>
        /// <param name="location"></param>
        /// <param name="numAvailablePositions"></param>
        /// <param name="lBL_ChargingDrone"></param>
        public Station(int id, string nameStation, Location location, int numAvailablePositions, IEnumerable<ChargingDrone> lBL_ChargingDrone)
        {
            Id = id;
            NameStation = nameStation;
            Location = location;
            NumAvailablePositions = numAvailablePositions;
            LBL_ChargingDrone = lBL_ChargingDrone;
        }

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