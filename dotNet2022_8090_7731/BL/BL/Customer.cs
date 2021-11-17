using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Customer
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location CLocation { get; set; }
        // שתי רשימות
        public List<ParcelInCustomer> lFromCustomer { get; set; }
        public List<ParcelInCustomer> lForCustomer { get; set; }
    }
}
