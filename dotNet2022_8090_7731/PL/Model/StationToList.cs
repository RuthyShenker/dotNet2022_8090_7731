using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    /// <summary>
    /// A class of StationToList that contains:
    /// Id,
    /// Name,
    /// AvailablePositions,
    /// FullPositions
    /// </summary>
    public class StationToList
    {
        /// <summary>
        /// A constructor that gets BO.StationToList and bulids 
        /// </summary>
        /// <param name="stationToList"></param>
        public StationToList(BO.StationToList stationToList)
        {
            Id = stationToList.Id;
            Name = stationToList.Name;
            AvailablePositions = stationToList.AvailablePositions;
            FullPositions = stationToList.FullPositions;
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
        public int AvailablePositions { get; set; }
        public int FullPositions { get; set; }

    }
}
