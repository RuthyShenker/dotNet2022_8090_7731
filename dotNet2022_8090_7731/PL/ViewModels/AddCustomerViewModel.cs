using PL.Model;
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
        public CustomerToAdd Customer { get; set; }
        public RelayCommand<object> AddCustomerCommand { get; set; }

        public AddCustomerViewModel(BlApi.IBL bl,Action refreshCustomerList)
        {
            Customer = new();
            this.bl = bl;
            this.refreshCustomer = refreshCustomerList;
            AddCustomerCommand = new RelayCommand<object>(AddCustomer);
        }

        private void AddCustomer(object obj)
        {
           //בדיקת תקינות+
           //להוסיף את היישות
        }
    }
}
