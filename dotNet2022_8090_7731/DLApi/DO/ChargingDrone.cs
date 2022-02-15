
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
        public int StationId { get; set; }
        public int DroneId { get; set; }
        public DateTime EnteranceTime { get; set; }
    }
}

