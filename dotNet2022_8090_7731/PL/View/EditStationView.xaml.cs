<<<<<<<< HEAD:dotNet2022_8090_7731/PL/View/EditStationView.xaml.cs
﻿using PL.ViewModels;
========
﻿using PL.View;
>>>>>>>> 8007f7459146c46af6ea81c15e73494ab0613a19:dotNet2022_8090_7731/PL/StationWindow.xaml.cs
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.View
{
    /// <summary>
    /// Interaction logic for EditStation.xaml
    /// </summary>
    public partial class EditStationView : UserControl
    {
        public EditStationView(EditStationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
