using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        public struct Customer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public Customer(string id, string name, string phone, double longitude, double latitude)
            {
                Id = id;
                Name = name;
                Phone = phone;
                Longitude = longitude;
                Latitude = latitude;
            }
            public override string ToString()
            {
                return $"Id: {Id} Name: {Name} Phone: {Phone}" +
                    $" Longitude: {Longitude} Latitude: {Latitude}";
            }
        }
    }
}
