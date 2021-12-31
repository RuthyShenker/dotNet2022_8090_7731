using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class EditCustomer
    {
        public EditCustomer(int id, string name, string phone, double longitude, double latitude,
            IEnumerable<ParcelInCustomer> lFromCustomer,IEnumerable<ParcelInCustomer> lForCustomer)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Location = new Location(longitude,latitude);
            LFromCustomer =lFromCustomer;
            LForCustomer =lForCustomer;
        }
        public int Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        // two lists
        public IEnumerable<ParcelInCustomer> LFromCustomer { get; set; }
        public IEnumerable<ParcelInCustomer> LForCustomer { get; set; }
    }
}
