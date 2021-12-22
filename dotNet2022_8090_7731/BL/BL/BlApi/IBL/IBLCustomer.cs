using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBLCustomer
    {
        int Add(Customer customer);
        void UpdatingCustomerDetails(int customerId, string newName, string newPhone);
        IEnumerable<CustomerToList> GetCustomers();
        Customer GetCustomer(int customerId);
    }
}
