using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerToList
    {
        public CustomerToList(int id, string name, string phone)
        {
            Id = id;
            Name = name;
            Phone = phone;
        }

        public int Id { get; set; }
        public string Name{ get; set; }
        public string Phone { get; set; }
        public int SentSupplied { get; set; }
        public int SentNotSupplied { get; set; }
        public int Got { get; set; }
        public int InWayToCustomer{ get; set; }
    }
}
