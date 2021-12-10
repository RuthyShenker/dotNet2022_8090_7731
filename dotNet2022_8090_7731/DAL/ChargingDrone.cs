using System;

namespace IDal
{
    namespace DO
    {
        /// <summary>
        /// A struct of ChargingDrone, contains:
        /// Id of Station,Id of Drone.
        /// </summary>
        public struct ChargingDrone: IDalObject
        {
            /// <summary>
            /// A constructor of ChargingDrone with fields.
            /// </summary>
            /// <param name="dId"></param>
            /// <param name="sId"></param>
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
