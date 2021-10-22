using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
         public struct BaseStation
         {
            public BaseStation(int id, string nameStation, int numberOfChargingStations, 
                double longitude, double latitude)
            {
                Id = id;
                NameStation = nameStation;
                NumAvailablePositions = numberOfChargingStations;
                Longitude = longitude;
                Latitude = latitude;
            }
            public BaseStation(BaseStation baseStation)
            {
                Id = baseStation.Id;
                NameStation = baseStation.NameStation;
                NumAvailablePositions = baseStation.NumAvailablePositions;
                Longitude = baseStation.Longitude;
                Latitude = baseStation.Latitude;
            }
            public int Id { get; init; }
            public string NameStation { get; set; }
            public int NumAvailablePositions { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return $"Station name: {NameStation}     Id: {Id}   Longitude: {Longitude}  Latitude: {Latitude}    " +
                    $"Number of available charging positions: {NumAvailablePositions}";
            }
        }
    }
}
