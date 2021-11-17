using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BL_Customer
    {
        public BL_Customer(string id, string name, string phone, Location cLocation)
        {
            Id = id;
            Name = name;
            Phone = phone;
            CLocation = cLocation;
        }
        public string Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location CLocation { get; set; }
        // שתי רשימות
        public List<ParcelInCustomer> lFromCustomer { get; set; }
        public List<ParcelInCustomer> lForCustomer { get; set; }
    }
}
