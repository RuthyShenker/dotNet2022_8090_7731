using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Model
{
    public class EditCustomer
    {
        public EditCustomer(int id, string name, string phone, Location location,
            IEnumerable<ParcelInCustomer> lFromCustomer,IEnumerable<ParcelInCustomer> lForCustomer)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Location = location;
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
