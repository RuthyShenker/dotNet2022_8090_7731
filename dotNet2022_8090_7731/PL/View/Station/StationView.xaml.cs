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
    /// Interaction logic for StationView.xaml
    /// </summary>
    public partial class StationView : Window
    {
        public StationView(BlApi.IBL bl)
        {
            InitializeComponent();

            var viewModel = new AddStationViewModel(bl, SwitchView);
            this.DataContext = new AddStationView(viewModel);
        }

        public StationView(BlApi.IBL bl, BO.Station selectedStation)
        {
            InitializeComponent();
            var viewModel = new EditStationViewModel(bl, selectedStation);
            this.DataContext = new EditStationView(viewModel);

        }

        private void SwitchView(BO.Station selectedStation)
        {
            var viewModel = new EditStationViewModel(BlApi.BlFactory.GetBl(), selectedStation);
            this.DataContext = new EditStationView(viewModel);
        }
    }
}
