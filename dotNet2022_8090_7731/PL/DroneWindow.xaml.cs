using BO;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {

        private IBL.IBL bl;

        public DroneWindow(IBL.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            MaxWeightComboBox.DataContext = Enum.GetValues(typeof(WeightCategories));
            StatusComboBox.DataContext = Enum.GetValues(typeof(DroneStatus));

        }
        public DroneWindow(IBL.IBL bl,DroneToList selectedDrone)
        {
            selectedDrone.BatteryStatus = 30;
            InitializeComponent();
            this.bl = bl;
            

        }
        private void Button_Click_Of_Adding_New_Drone(object sender, RoutedEventArgs e)
        {
            ///exist?
            ///בדיקת תקינות 
            ///תחנה להטענה
            ///delivery?
            int id;
            int.TryParse(IdTextBox.Text, out id);
            float batteryStatus;
            float.TryParse(BatteryTextBox.Text, out batteryStatus);
            string model = ModelTextBox.Text;
            double longitude;
            double latitude;
            double.TryParse(LongitudeTextBox.Text, out longitude);
            double.TryParse(LatitudeTextBox.Text, out latitude);
            WeightCategories weight = (WeightCategories)MaxWeightComboBox.SelectedIndex;
            DroneStatus droneStatus = (DroneStatus)StatusComboBox.SelectedIndex;
            Drone newDrone = new Drone(id, model, weight, batteryStatus, droneStatus, null, new Location(longitude, latitude));
            bl.AddingDrone(newDrone, 0);
        }

        private void Close_Drone_Window_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


