using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    ///A class of CustomerToList that contains:
    ///Id
    ///Name
    ///Phone
    ///SentSupplied
    ///SentNotSupplied
    ///Got
    ///InWayToCustomer
    /// </summary>
    public class CustomerToList
    {
        /// <summary>
        /// this field is init. 
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SentSupplied { get; set; }
        public int SentNotSupplied { get; set; }
        public int Got { get; set; }
        public int InWayToCustomer { get; set; }
        public override string ToString()
        {
            return BL.Tools.ToStringProps(this);
        }
    }
}
