using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ViewModels
{
    public class AddCustomerViewModel
    {
        BlApi.IBL bl;
        Action refreshCustomer;
        public AddCustomerViewModel(BlApi.IBL bl,Action refreshCustomerList)
        {
            this.bl = bl;
        }
    }
}
