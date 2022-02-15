using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// A class of CustomerInParcel that 
    /// contains:
    /// Id
    ///Name
    /// </summary>
    public class CustomerInParcel
    { 
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}


