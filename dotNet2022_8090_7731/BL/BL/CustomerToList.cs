using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerToList
    {
        public string Id { get; set; }
        public string Name{ get; set; }
        public string Phone { get; set; }
        public int SentSupplied { get; set; }
        public int SentNotSupplied { get; set; }
        public int Got { get; set; }
        public int InWayToCustomer{ get; set; }
    }
}
