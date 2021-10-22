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
            public Customer(string id, string name, string phone, double longitude, double latitude)
            {
                Id = id;
                Name = name;
                Phone = phone;
                Longitude = longitude;
                Latitude = latitude;
            }
            public Customer(Customer customer)
            {
                Id = customer.Id;
                Name = customer.Name;
                Phone = customer.Phone;
                Longitude = customer.Longitude;
                Latitude = customer.Latitude;
            }
            public string Id { get; init; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

            public override string ToString()
            {
                return $"Name: {Name}   Id: {Id}    Phone: {Phone}  " +
                    $"Longitude: {Longitude}    Latitude: {Latitude}";
            }
        }
    }
}
