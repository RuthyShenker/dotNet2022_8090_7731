using System;


namespace DO
{
    /// <summary>
    /// A struct of ChargingDrone that impliments IDalDo, contains:
    /// Id of Station,Id of Drone,EnteranceTime to charging.
    /// </summary>
    [Serializable]
    public struct ChargingDrone : IDalDo
    {
        public int StationId { get; set; }
        public int DroneId { get; set; }
        public DateTime EnteranceTime { get; set; }
    }
}

