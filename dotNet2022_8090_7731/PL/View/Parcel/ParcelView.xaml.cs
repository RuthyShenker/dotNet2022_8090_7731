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
    /// Interaction logic for ParcelView.xaml
    /// </summary>
    public partial class ParcelView : Window
    {
        public ParcelView(BlApi.IBL bl/*, Action refreshParcelList*/)
        {
            InitializeComponent();
            //DroneView.DataContext = new AddNewDroneView(bl, refreshDroneList,Close);
            //var viewModel = new AddDroneViewModel(/*bl, */refreshDroneList);
            //this.DataContext = new AddDroneView(viewModel);
            var viewModel = new AddParcelViewModel(bl, SwitchView);
            this.DataContext = new AddParcelView(viewModel);
        }

        public ParcelView(BlApi.IBL bl, /*Action refreshParcelList,*/ BO.Parcel selectedParcel)
        {
            InitializeComponent();
            //DroneView.DataContext= new EditDroneView(bl, initializeDrones, selectedDrone,Close);
            var viewModel = new EditParcelViewModel(bl, selectedParcel);
            this.DataContext = new EditParcelView(viewModel);
        }
        private void SwitchView(BO.Parcel selectedParcel)
        {
            //refreshCustomerList();
            var viewModel = new EditParcelViewModel(BlApi.BlFactory.GetBl(), selectedParcel);
            this.DataContext = new EditParcelView(viewModel);
        }
    }
}
