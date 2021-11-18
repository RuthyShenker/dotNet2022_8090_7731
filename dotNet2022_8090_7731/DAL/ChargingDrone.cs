using System;

namespace IDal
{
    namespace DO
    {
        /// <summary>
        /// A struct of ChargingDrone, contains:
        /// Id of Station,Id of Drone.
        /// </summary>
        public struct ChargingDrone
        {
            public ChargingDrone(int dId, int sId)
            {
                DroneId = dId;
                StationId = sId;
            }

            public int StationId{ get; set; }
            public int DroneId{ get; set; }
        }
    }
}
