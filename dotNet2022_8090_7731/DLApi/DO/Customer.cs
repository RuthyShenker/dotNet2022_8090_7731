
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// A struct of Customer contains:
    /// Id,Name,Phone,Longitude,Latitude
    /// </summary>
    [Serializable]
    public struct Customer : IIdentifiable, IDalObject
    {
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
    }
}

