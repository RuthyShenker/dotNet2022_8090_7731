
using System;

namespace DO
{
    /// <summary>
    /// A struct of ChargingDrone, contains:
    /// Id of Station,Id of Drone.
    /// </summary>
    [Serializable]
    public struct ChargingDrone : IDalObject
    {
        /// <summary>
        /// A constructor of ChargingDrone with fields.
        /// </summary>
        /// <param name="dId"></param>
        /// <param name="sId"></param>
        public ChargingDrone(int dId, int sId, DateTime date)
        {
            DroneId = dId;
            StationId = sId;
            EnteranceTime = date;
        }

        public int StationId { get; set; }
        public int DroneId { get; set; }
        public DateTime EnteranceTime { get; set; }
    }
}

