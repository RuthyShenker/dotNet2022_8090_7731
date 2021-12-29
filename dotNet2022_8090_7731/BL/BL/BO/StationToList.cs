using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
        public StationToList(int id, string name, int availablePositions, int fullPositions)
        {
            Id = id;
            Name = name;
            AvailablePositions = availablePositions;
            FullPositions = fullPositions;
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
        public int AvailablePositions { get; set; }
        public int FullPositions { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }

    }
}


