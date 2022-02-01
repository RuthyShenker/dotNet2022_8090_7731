﻿using PL.ViewModels;
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
    /// Interaction logic for DroneView.xaml
    /// </summary>
    public partial class DroneView : Window
    {
        public DroneView(/*BlApi.IBL bl,*//* Action refreshDroneList*/)
        {
            //var viewModel = new AddDroneViewModel(/*bl,*/ /*refreshDroneList*/);
            var viewModel = new AddDroneViewModel(SwitchView);
            this.DataContext = new AddDroneView(viewModel);
            InitializeComponent();
            //DroneView.DataContext = new AddNewDroneView(bl, refreshDroneList,Close);
            //var viewModel = new AddDroneViewModel(/*bl, */refreshDroneList);
            //this.DataContext = new AddDroneView(viewModel);

        }

        public DroneView(BlApi.IBL bl/*, Action refreshDrones*/, BO.Drone selectedDrone)
        {
            InitializeComponent();
            //DroneView.DataContext= new EditDroneView(bl, initializeDrones, selectedDrone,Close);
            var viewModel = new EditDroneViewModel(bl, selectedDrone);
            this.DataContext = new EditDroneView(viewModel);
        }

        private void SwitchView(BO.Drone selectedDrone)
        {
            //refreshCustomerList();
            var viewModel = new EditDroneViewModel(BlApi.BlFactory.GetBl(), selectedDrone);
            this.DataContext = new EditDroneView(viewModel);
        }
    }
}
