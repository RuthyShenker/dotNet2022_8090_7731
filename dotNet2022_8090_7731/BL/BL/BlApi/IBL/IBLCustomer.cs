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
        //ADD:
        int AddCustomer(Customer customer);

        //UPDATE:
        void UpdatingCustomerDetails(int customerId, string newName, string newPhone);

        //GET:
        IEnumerable<CustomerToList> GetCustomers();/*Func<int, bool> predicate = null*/
        Customer GetCustomer(int customerId);

        //DELETE:
        string DeleteCustomer(int customerId);
    }
}
