using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class BL_Customer
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location CLocation{ get; set; }
        // שתי רשימות
        public List<ParcelInCustomer> lFromCustomer;
        public List<ParcelInCustomer> lForCustomer;
    }
}
