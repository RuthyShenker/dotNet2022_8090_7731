using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    namespace DO
    {
        /// <summary>
        /// A struct of Customer contains:
        /// Id,Name,Phone,Longitude,Latitude
        /// </summary>
        public struct Customer:IIdentifiable
        {
            /// <summary>
            /// A constructor of Customer that gets parameters 
            /// and initalizes the new instance with this 
            /// parameters.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="phone"></param>
            /// <param name="longitude"></param>
            /// <param name="latitude"></param>
            public Customer(int id, string name, string phone, double longitude, double latitude)
            {
                Id = id;
                Name = name;
                Phone = phone;
                Longitude = longitude;
                Latitude = latitude;
            }
            /// <summary>
            /// A constructor of Custoner that gets 
            /// an instance of Customer and initalizes
            /// the new instance with the parameters of this instance.
            /// </summary>
            /// <param name="customer"></param>
            public Customer(Customer customer)
            {
                Id = customer.Id;
                Name = customer.Name;
                Phone = customer.Phone;
                Longitude = customer.Longitude;
                Latitude = customer.Latitude;
            }
            /// <summary>
            /// this field is init.
            /// </summary>
            public int Id { get; init; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

            /// <summary>
            /// A function that returns the details of the customer
            /// </summary>
            /// <returns>The details</returns>
            public override string ToString()
            {
                return $"Name: {Name}   Id: {Id}    Phone: {Phone}  " +
                    $"Longitude: {Longitude}    Latitude: {Latitude}";
            }
            /// <summary>
            /// A function that returns new customer initalizes
            /// with the parameters of the instance that the function worked on.
            /// </summary>
            /// <returns></returns>
            public Customer Clone()
            {
                return new Customer()
                {
                    Id = Id,
                    Name=Name,
                    Phone=Phone,
                    Longitude=Longitude,
                    Latitude=Latitude
                };
            }

        }
    }
}
