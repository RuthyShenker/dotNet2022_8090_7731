﻿using IBL.BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBL bl;
        public DroneListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            DataContext = bl.GetListToList<IDal.DO.Drone,DroneToList>();
            Weights.DataContext=
        }

        private void Weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Weights.SelectedItem == null)
            {
                DroneListView.ItemsSource = bl.GetListToList<IDal.DO.Drone,DroneToList>();
            }
            else
            {
                WeightCategories weight = (WeightCategories)Weights.SelectedItem;
                DroneListView.ItemsSource = bl.GetListToList<IDal.DO.Drone, DroneToList>(drone => drone.MaxWeight==weight);
            }
        }
    }
}
