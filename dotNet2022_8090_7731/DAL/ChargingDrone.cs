using System;

namespace DAL
{
    namespace DO
    {
        /// <summary>
        /// A struct of ChargingDrone, contains:
        /// Id of Station,Id of Drone.
        /// </summary>
        public struct ChargingDrone
        {
            public int StationId{ get; set; }
            public int DroneId{ get; set; }
        }
    }
}
