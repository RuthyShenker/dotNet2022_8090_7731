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
            public BaseStation(string id, string nameStation, int numberOfChargingStations, 
                double longitude, double latitude)
            {
                Id = id;
                NameStation = nameStation;
                NumAvailablePositions = numberOfChargingStations;
                Longitude = longitude;
                Latitude = latitude;
            }
            public string Id { get; set; }
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
