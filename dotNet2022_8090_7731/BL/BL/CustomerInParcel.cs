using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// A class of CustomerInParcel that 
    /// contains:
    /// Id
    ///Name
    /// </summary>
    public class CustomerInParcel
    {
        public CustomerInParcel(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
       
       
    }

}
