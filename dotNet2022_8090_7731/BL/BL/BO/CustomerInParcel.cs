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
        ///A constructor of CustomerInParcel with fields.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public CustomerInParcel(int id, string name)
        {
            Id = id;
            Name = name;
        }
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public string Name { get; set; }
    }
}


