using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// A struct of base station,contains:
    /// Id,Name of Station,Number of Available Positions,Longitude,Latitude.
    /// </summary>
    public struct BaseStation : IIdentifiable, IDalObject
    {
        /// <summary>
        /// A constructor of base station that gets parameters 
        /// and initalizes the new instance with this 
        /// parameters.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameStation"></param>
        /// <param name="numberOfChargingStations"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public BaseStation(int id, string nameStation, int numberOfChargingStations,
            double longitude, double latitude)
        {
            Id = id;
            NameStation = nameStation;
            NumberOfChargingPositions = numberOfChargingStations;
            Longitude = longitude;
            Latitude = latitude;
        }
        /// <summary>
        /// A constructor of base station that gets 
        /// an instance of base station and initalizes
        /// the new instance with the parameters of this instance.
        /// </summary>
        /// <param name="baseStation"></param>
        public BaseStation(BaseStation baseStation)
        {
            Id = baseStation.Id;
            NameStation = baseStation.NameStation;
            NumberOfChargingPositions = baseStation.NumberOfChargingPositions;
            Longitude = baseStation.Longitude;
            Latitude = baseStation.Latitude;
        }


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
            return $"Station name: {NameStation}     Id: {Id}   Longitude: {Longitude}  Latitude: {Latitude}    " +
                $"Number of charging positions: {NumberOfChargingPositions}";
        }
    }
}

