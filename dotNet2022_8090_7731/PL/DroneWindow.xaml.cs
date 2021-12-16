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
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

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
            GridOfAddDrone.Visibility = Visibility.Visible;
            GridOfUpdateDrone.Visibility = Visibility.Collapsed;
            detailsOfDrone.DataContext = new DroneToList();
        }
        public DroneWindow(IBL.IBL bl, DroneToList selectedDrone)
        {
            InitializeComponent();
            this.bl = bl;
            MessageBox.Show($"{ selectedDrone.Id}", $"{selectedDrone.Model}");
            GridOfAddDrone.Visibility = Visibility.Collapsed;
            GridOfUpdateDrone.Visibility = Visibility.Visible;
        }

        private void Button_Click_Of_Adding_New_Drone(object sender, RoutedEventArgs e)
        {

            CheckValidDrone((DroneToList)detailsOfDrone.DataContext, (Button)sender);

            //int id;
            //int.TryParse(IdTextBox.Text, out id);
            //float batteryStatus;
            //float.TryParse(BatteryTextBox.Text, out batteryStatus);
            //string model = ModelTextBox.Text;
            //double longitude;
            //double latitude;
            //double.TryParse(LongitudeTextBox.Text, out longitude);
            //double.TryParse(LatitudeTextBox.Text, out latitude);
            //WeightCategories weight = (WeightCategories)MaxWeightComboBox.SelectedIndex;
            //DroneStatus droneStatus = (DroneStatus)StatusComboBox.SelectedIndex;
            //Drone newDrone = new Drone(id, model, weight, batteryStatus, droneStatus, null, new Location(longitude, latitude));
            ////bl.AddingDrone(newDrone, 0);  

            ///תחנה להטענה
            ///delivery?
        }

        private void CheckValidDrone(DroneToList drone, Button sender)
        {

            MessageBox.Show($"{drone.Model},{drone.CurrLocation.Longitude } {drone.CurrLocation}, {drone.CurrLocation.Latitude}");
            bool isExist = bl.GetDrones().Any(d => d.Id == drone.Id);
            if (isExist)
            {
                var textBox = (TextBox)message.FindName("IdTextBox");
                AddToolTip(textBox, " Id is not available ");
                return;
            }

            if (drone.CurrLocation.Longitude is < (-90) or > 90)
            {
                var textBox = (TextBox)message.FindName("Longitude");
                AddToolTip(textBox, "InCorrect longitude (-90, 90) ");
                return;
            }

            if (drone.CurrLocation.Latitude is < (-90) or > 90)
            {
                var textBox = (TextBox)message.FindName("Latitude");
                AddToolTip(textBox, "InCorrect Latitude (-90, 90) ");
                return;
            }



            // to finish
        }

        private void AddToolTip(TextBox textBox, string str)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = str;
            toolTip.IsOpen = true;
            textBox.ToolTip = toolTip;

            // Turn off ToolTip
            DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1), IsEnabled = true };
            timer.Tick += new EventHandler(delegate (object timerSender, EventArgs timerArgs)
            {
                toolTip.IsOpen = false;
                timer = null;
            });
        }

        void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (e.Handled = new Regex("[^0-9]+").IsMatch(e.Text))
            {
                textBox.Background = Brushes.Gray;
                ToolTip toolTip = new ToolTip();
                AddToolTip(textBox, " Input has to contain only digits ");
            }
            else
            {
                textBox.Background = Brushes.White;
            }
        }
        //private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!IdTextBox.Text.All(ch => char.IsNumber(ch)))
        //    {
        //        TextBox textBox = (TextBox)detailsOfDrone.FindName("IdTextBox");
        //        textBox.BorderBrush = Brushes.Red;
        //    }
        //}

        private void MaxWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        //        IdTextBox
        //BatteryTextBox
        //MaxWeightComboBox
        //ModelTextBox
        //StatusComboBox
        //DeliveryTextBox
        //LatitudeTextBox
        //LongitudeTextBox

        //private void Close_Drone_Window_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}
    }
}



