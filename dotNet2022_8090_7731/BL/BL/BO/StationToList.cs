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
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
        public int AvailablePositions { get; set; }
        public int FullPositions { get; set; }

        /// <summary>
        /// A function that returns the details of this Station To List
        /// </summary>
        /// <returns> the details of this Station To List</returns>
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }

    }
}


