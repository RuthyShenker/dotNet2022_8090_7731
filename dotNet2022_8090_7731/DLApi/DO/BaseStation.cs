using System;


namespace DO
{
    /// <summary>
    /// A struct of base station that implements IIdentifiable, IDalDo, contains:
    /// Id,Name of Station,Number of Available Positions,Longitude,Latitude.
    /// </summary>
    [Serializable]
    public struct BaseStation : IIdentifiable, IDalDo
    {
        /// <summary>
        /// this field is init
        /// </summary>
        public int Id { get; init; }
        public string NameStation { get; set; }
        public int NumberOfChargingPositions { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        /// <summary>
        /// A function that returns the details of the base station.
        /// </summary>
        /// <returns>the details</returns>
        public override string ToString()
        {
            return $"Station name: {NameStation}  Id: {Id}  Longitude: {Longitude}  Latitude: {Latitude}    " +
                $"Number of charging positions: {NumberOfChargingPositions}";
        }
    }
}

