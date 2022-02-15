using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    ///A class of ParcelToList that contains:
    ///Id
    ///SenderName
    ///GetterName
    ///Weight
    ///MyPriority
    ///Status
    /// </summary>
    public class ParcelToList
    {
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string SenderName { get; set; }
        public string GetterName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority MyPriority { get; set; }
        public ParcelStatus Status { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }

    }
}

