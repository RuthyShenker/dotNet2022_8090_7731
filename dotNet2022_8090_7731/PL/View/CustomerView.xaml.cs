using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window
    {
        public CustomerView(BlApi.IBL bl, Action refreshCustomerList)
        {
            InitializeComponent();
            var viewModel = new AddCustomerViewModel(bl, refreshCustomerList);
            this.DataContext = new AddCustomerView(viewModel);
        }

        public CustomerView(BlApi.IBL bl, Action refreshCustomerList, BO.Customer selectedCustomer)
        {
            InitializeComponent();
            var viewModel = new EditCustomerViewModel(bl, selectedCustomer, refreshCustomerList);
            this.DataContext = new EditCustomerView(viewModel);
        }
    }
}
